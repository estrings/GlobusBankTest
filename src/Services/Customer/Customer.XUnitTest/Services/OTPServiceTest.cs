using Customer.API.Common.Communication;
using Customer.API.Common.Helpers;
using Customer.API.Data.Entities;
using Customer.API.Data.Enums;
using Customer.API.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.XUnitTest.Services
{
    public class OTPServiceTest : IOTPService
    {
        private readonly List<UserOTP> _dbUserOTP;

        public OTPServiceTest()
        {
            //_dbUserOTP = new List<UserOTP>(); // use this when adding creating new OTP
            _dbUserOTP = new List<UserOTP>()
            {
                new UserOTP() { Created = DateTime.UtcNow, Id = 1, IsDeleted = false, OTP = "12345", PhoneNumber = "08167970254", Status = OTPStatus.UNUSED}
            }; // use this when verifying OTP
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

                    var resp = await Task.FromResult(ExecutedResult<string>.Success("mock", "Request successful")); ;

                    if (!string.IsNullOrWhiteSpace(resp.Message) && resp.Result == "mock")
                    {
                        respMessage = respMessage + $" Use TEST OTP {addUserOTP.Result}.";
                    }

                    return ExecutedResult<string>.Success(respMessage, "Request successful");
                }
                else
                    return ExecutedResult<string>.NotCompleted(addUserOTP.Message, "Request not completed");
            }
            catch (Exception)
            {
                return ExecutedResult<string>.Exception("Something went wrong.");
            }
        }

        public async Task<ExecutedResult<string>> AddUserOTP(string phoneNumber)
        {
            try
            {
                await DeleteExistingOTP(phoneNumber); // deleting it just for test purpose. That should not been done is a real application
                var otp = "12345";
                int count = _dbUserOTP.Count;
                UserOTP userOTP = new UserOTP
                {
                    Id = count + 1,
                    PhoneNumber = phoneNumber,
                    Status = OTPStatus.UNUSED,
                    Created = DateTime.Now,
                    OTP = otp
                };

                _dbUserOTP.Add(userOTP);
                return ExecutedResult<string>.Success(otp, "Request successful");
            }
            catch (Exception)
            {
                return ExecutedResult<string>.Exception("Something went wrong");
            }
        }

        public async Task DeleteExistingOTP(string phoneNumber)
        {
            try
            {
                var existingOTPs = _dbUserOTP.Where(x => x.PhoneNumber == phoneNumber);

                if (existingOTPs != null && existingOTPs.Count() > 0)
                {
                    foreach (var record in existingOTPs)
                    {
                        record.IsDeleted = true;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task<ExecutedResult<string>> ValidateOTP(string phoneNumber, string otp)
        {
            try
            {
                var userOTP = _dbUserOTP.FirstOrDefault(x => x.PhoneNumber == phoneNumber && x.OTP == otp);

                if (userOTP != null)
                {
                    userOTP.Status = OTPStatus.USED;
                    userOTP.IsDeleted = true;
                    //await _unitOfWork.Repository<UserOTP>().UpdateAsync(userOTP);
                    var sessionId = Guid.NewGuid().ToString();
                    return await Task.FromResult(ExecutedResult<string>.Success(sessionId, "OTP Validated successfully."));
                }
                else
                    return ExecutedResult<string>.NotCompleted("Invalid details");
            }
            catch (Exception)
            {
                return ExecutedResult<string>.Exception("Something went wrong");
            }
        }
    }
}
