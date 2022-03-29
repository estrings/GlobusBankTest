using Customer.API.Common.Communication;
using Customer.API.Common.Helpers;
using Customer.API.Data.Entities;
using Customer.API.Data.Enums;
using Customer.API.Data.UnitOfWork;
using Customer.API.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Customer.API.Service.Implementation
{
    public class OTPService : IOTPService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly ILogger<OTPService> _logger;
        private readonly ISMSService _smsService;
        public OTPService(IUnitofWork unitOfWork, ILogger<OTPService> logger, ISMSService smsService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _smsService = smsService;
        }

        public async Task DeleteExistingOTP(string phoneNumber)
        {
            try
            {
                var existingOTPs = await _unitOfWork.Repository<UserOTP>().FindAllAsync(x => x.PhoneNumber == phoneNumber);

                if (existingOTPs != null && existingOTPs.Count > 0)
                {
                    foreach (var record in existingOTPs)
                    {
                        record.IsDeleted = true;
                        await _unitOfWork.Repository<UserOTP>().UpdateAsync(record);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OTPService: An error occured when deleting existing OTP");
            }
        }

        public async Task<ExecutedResult<string>> SendOTP(string phoneNumber)
        {
            try
            {
                var addUserOTP = await AddUserOTP(phoneNumber);
                var respMessage = $"A One Time Password (OTP) has been sent to ({Utility.MaskPhoneNumber(phoneNumber)}).";
                if (addUserOTP.Response == ResponseCode.Ok)
                {
                    string message = $"Use this One Time Password(OTP) to confirm your request {addUserOTP.Result}";
                    var resp = await _smsService.SendSMSAsync(phoneNumber, message);

                    if (!string.IsNullOrWhiteSpace(resp.Message) && resp.Result == "mock")
                    {
                        respMessage = respMessage + $" Use TEST OTP {addUserOTP.Result}.";
                    }

                    return ExecutedResult<string>.Success(respMessage, "Request successful");
                }
                else
                    return ExecutedResult<string>.NotCompleted(addUserOTP.Message, "Request not completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OTPService: An error occured when sending OTP");
                return ExecutedResult<string>.Exception("Something went wrong.");
            }
        }

        public async Task<ExecutedResult<string>> AddUserOTP(string phoneNumber)
        {
            try
            {
                await DeleteExistingOTP(phoneNumber); // deleting it just for test purpose. That should not been done is a real application
                var otp = Utility.GenerateOTP();
                UserOTP userOTP = new UserOTP
                {
                    PhoneNumber = phoneNumber,
                    Status = OTPStatus.UNUSED,
                    Created = DateTime.Now,
                    OTP = otp
                };

                await _unitOfWork.Repository<UserOTP>().AddAsync(userOTP);
                return ExecutedResult<string>.Success(otp, "Request successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OTPService: An error occured when saving OTP to database");
                return ExecutedResult<string>.Exception("Something went wrong");
            }
        }

        public async Task<ExecutedResult<string>> ValidateOTP(string phoneNumber, string otp)
        {
            try
            {
                var userOTP = await _unitOfWork.Repository<UserOTP>().FindAsync(x => x.PhoneNumber == phoneNumber && x.OTP == otp);

                if (userOTP != null)
                {
                        userOTP.Status = OTPStatus.USED;
                        userOTP.IsDeleted = true;
                        await _unitOfWork.Repository<UserOTP>().UpdateAsync(userOTP);
                        var sessionId = Guid.NewGuid().ToString();
                        return ExecutedResult<string>.Success(sessionId, "OTP Validated successfully.");                   
                }
                else
                    return ExecutedResult<string>.NotCompleted("Invalid details");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OTPService: An error occured when validating OTP.");
                return ExecutedResult<string>.Exception("Something went wrong");
            }
        }
    }
}
