using Customer.API.Common.Communication;
using Customer.API.Common.Extensions;
using Customer.API.Common.Helpers;
using Customer.API.Common.ResourceParameters;
using Customer.API.Data.Entities;
using Customer.API.Data.Enums;
using Customer.API.Data.UnitOfWork;
using Customer.API.Service.Dtos.Request;
using Customer.API.Service.Dtos.Response;
using Customer.API.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.API.Service.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CustomerService> _logger;
        private readonly IUnitofWork _unitOfWork;
        private readonly IOTPService _oTPService;
        public CustomerService(IConfiguration config, ILogger<CustomerService> logger, IUnitofWork unitOfWork, IOTPService oTPService)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _oTPService = oTPService;
        }

        public async Task<ExecutedResult<CustomerResponseDto>> CreateCustomer(CustomerRequestDto customerRequestDto)
        {
            try
            {
                var result = await _unitOfWork.Repository<Data.Entities.Customer>().FindAsync(c => c.PhoneNumber == customerRequestDto.phoneNumber && c.Email == customerRequestDto.email);
                if (result != null) return ExecutedResult<CustomerResponseDto>.NotCompleted($"Customer with phone number: {customerRequestDto.phoneNumber} and email address: {customerRequestDto.email} already exists");

                Data.Entities.Customer customer = new Data.Entities.Customer()
                {
                    IsAccountActive = false,
                    LGA = customerRequestDto.lga,
                    Email = customerRequestDto.email,
                    Password = Cryptography.Encrypt(customerRequestDto.password),
                    PhoneNumber = customerRequestDto.phoneNumber,
                    State = customerRequestDto.state
                };

                //send customer OTP
                var otpResp = await _oTPService.SendOTP(customerRequestDto.phoneNumber);
                if (otpResp.Response == ResponseCode.Ok)
                {
                    var response = await _unitOfWork.Repository<Data.Entities.Customer>().AddAsync(customer);
                    var customerResponse = new CustomerResponseDto()
                    {
                        Id = response.Id,
                        message = $"Kindly use test OTP {otpResp.Result}"
                    };
                    return ExecutedResult<CustomerResponseDto>.Success(customerResponse, "Request successful");
                }
                else
                    return ExecutedResult<CustomerResponseDto>.Failed($"{otpResp.Result}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CustomerService: An error occured when creating customer");
                return ExecutedResult<CustomerResponseDto>.Exception("Something went wrong");
            }
        }

        public async Task<ExecutedResult<ExtendedCustomerResponseDto>> Customers(ResourceParameters resourceParameters)
        {
            try
            {
                var customer = await _unitOfWork.Repository<Data.Entities.Customer>().GetAllAsync();
                if (customer.Any())
                {
                    var mapCustomers = customer.Select(c => new CustomersResponseDto
                    {
                        isAccountActive = c.IsAccountActive,
                        email = c.Email,
                        phoneNumber = c.PhoneNumber,
                        lga = GetStateLga(c.Id).Result.Item2,
                        state = GetStateLga(c.Id).Result.Item1
                    }).AsQueryable();

                    var pageList = PageList<CustomersResponseDto>.Create(mapCustomers, resourceParameters.PageNumber, resourceParameters.PageSize);
                    var count = customer.Count;
                    var response = new ExtendedCustomerResponseDto
                    {
                        items = pageList,
                        TotalCount = count
                    };
                    return ExecutedResult<ExtendedCustomerResponseDto>.Success(response, "Request Successful");
                }

                var response2 = new ExtendedCustomerResponseDto
                {
                    items = null,
                    TotalCount = customer.Count
                };
                return ExecutedResult<ExtendedCustomerResponseDto>.NotFound("Request Successful. No record found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CustomerService: An error occured when getting all customers");
                return ExecutedResult<ExtendedCustomerResponseDto>.Exception("Something went wrong");
            }
        }

        public async Task<ExecutedResult<VerifyCustomerResponseDto>> VerifyCustomer(VerifyCustomerRequestDto verifyCustomerRequestDto)
        {
            try
            {
                if(verifyCustomerRequestDto.customerId < 0) return ExecutedResult<VerifyCustomerResponseDto>.Failed("Invalid customer Id");

                var custDetails = await _unitOfWork.Repository<Data.Entities.Customer>().FindAsync(c => c.Id == verifyCustomerRequestDto.customerId);
                if (custDetails is null) return ExecutedResult<VerifyCustomerResponseDto>.Failed("Customer does not exists");


                var result = await _oTPService.ValidateOTP(verifyCustomerRequestDto.phoneNumber, verifyCustomerRequestDto.otp);
                if(result.Response != ResponseCode.Ok) return ExecutedResult<VerifyCustomerResponseDto>.Failed("Could not validate OTP");

                custDetails.IsAccountActive = true;
                await _unitOfWork.Repository<Data.Entities.Customer>().UpdateAsync(custDetails);

                var response = new VerifyCustomerResponseDto()
                {
                    email = custDetails.Email,
                    phoneNumber = custDetails.PhoneNumber
                };
               

                return ExecutedResult<VerifyCustomerResponseDto>.Success(response, "Customer details verified successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CustomerService: An error occured when validating customer OTP");
                return ExecutedResult<VerifyCustomerResponseDto>.Exception("Something went wrong");
            }
        }

        private async Task<Tuple<string, string>> GetStateLga(long lgaId)
        {
            var result1 = await _unitOfWork.Repository<LGA>().FindAsync(s => s.Id == lgaId);
            var result2 = await _unitOfWork.Repository<State>().FindAsync(s => s.Id == result1.StateId);
            return new Tuple<string, string>(result2.Name, result1.Name);
        }
    }
}
