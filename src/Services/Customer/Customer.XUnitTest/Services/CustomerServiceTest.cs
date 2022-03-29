using Customer.API.Common.Communication;
using Customer.API.Common.Extensions;
using Customer.API.Common.Helpers;
using Customer.API.Common.ResourceParameters;
using Customer.API.Data.Entities;
using Customer.API.Data.Enums;
using Customer.API.Service.Dtos.Request;
using Customer.API.Service.Dtos.Response;
using Customer.API.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.XUnitTest.Services
{
    public class CustomerServiceTest : ICustomerService
    {
        private List<API.Data.Entities.Customer> _dbCustomers = new List<API.Data.Entities.Customer>();
        private readonly List<LGA> _dbLga;
        private readonly List<State> _dbStates;
        private readonly IOTPService _oTPService;

        public CustomerServiceTest()
        {
            _dbCustomers = new List<API.Data.Entities.Customer>()
            {
                new API.Data.Entities.Customer() {Id = 1, Email = "trustlead01@gmail.com", IsAccountActive = false, Password="pass12c8iowp*7yh", LGA = 1, State = 1, PhoneNumber = "08167970254"},
                new API.Data.Entities.Customer() {Id = 2, Email = "trustlead01@gmail.com", IsAccountActive = true, Password="_1ss12c8iowp*7yh", LGA = 1, State = 1, PhoneNumber = "08123970254"},
            };

            _dbLga = new List<LGA>()
            {
                new LGA() {Id = 1, Name = "Lagos Island", StateId = 1},
                new LGA() {Id = 2, Name = "Agege", StateId = 1},
                new LGA() {Id = 3, Name = "Delta", StateId = 2},
            };

            _dbStates = new List<State>()
            {
                new State() {Id = 1, Name = "Lagos", Lga = null},
                new State() {Id = 2, Name = "Delta", Lga = null},
            };

            _oTPService = new OTPServiceTest();
        }
        public async Task<ExecutedResult<CustomerResponseDto>> CreateCustomer(CustomerRequestDto customerRequestDto)
        {
            try
            {
                var result = _dbCustomers.FirstOrDefault(c =>  c.PhoneNumber == customerRequestDto.phoneNumber && c.Email == customerRequestDto.email);
                if (result != null) 
                    return await Task.FromResult(ExecutedResult<CustomerResponseDto>.NotCompleted($"Customer with phone number: {customerRequestDto.phoneNumber} and email address: {customerRequestDto.email} already exists"));

                API.Data.Entities.Customer customer = new API.Data.Entities.Customer()
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
                    _dbCustomers.Add(customer);
                    int count = _dbCustomers.Count;
                    var customerResponse = new CustomerResponseDto()
                    {
                        Id = count,
                        message = $"Kindly use test OTP {otpResp.Result}"
                    };
                    return ExecutedResult<CustomerResponseDto>.Success(customerResponse, "Request successful");
                }
                else
                    return ExecutedResult<CustomerResponseDto>.Failed($"{otpResp.Result}");
            }
            catch (Exception)
            {
                return ExecutedResult<CustomerResponseDto>.Exception("Something went wrong");
            }
        }

        public async Task<ExecutedResult<ExtendedCustomerResponseDto>> Customers(ResourceParameters resourceParameters)
        {
            try
            {
                var customer = _dbCustomers;
                if (customer.Any())
                {
                    var mapCustomers = customer.Select(c => new CustomersResponseDto
                    {
                        isAccountActive = c.IsAccountActive,
                        email = c.Email,
                        phoneNumber = c.PhoneNumber,
                        lga = GetStateLga(c.Id).Item2,
                        state = GetStateLga(c.Id).Item1
                    }).AsQueryable();

                    var pageList = PageList<CustomersResponseDto>.Create(mapCustomers, resourceParameters.PageNumber, resourceParameters.PageSize);
                    var count = customer.Count;
                    var response = new ExtendedCustomerResponseDto
                    {
                        items = pageList,
                        TotalCount = count
                    };
                    return await Task.FromResult(ExecutedResult<ExtendedCustomerResponseDto>.Success(response, "Request Successful"));
                }

                var response2 = new ExtendedCustomerResponseDto
                {
                    items = null,
                    TotalCount = customer.Count
                };
                return await Task.FromResult(ExecutedResult<ExtendedCustomerResponseDto>.NotFound("Request Successful. No record found"));
            }
            catch (Exception)
            {
                return ExecutedResult<ExtendedCustomerResponseDto>.Exception("Something went wrong");
            }
        }

        public async Task<ExecutedResult<VerifyCustomerResponseDto>> VerifyCustomer(VerifyCustomerRequestDto verifyCustomerRequestDto)
        {
            try
            {
                if (verifyCustomerRequestDto.customerId < 0) return ExecutedResult<VerifyCustomerResponseDto>.Failed("Invalid customer Id");

                var custDetails = _dbCustomers.FirstOrDefault(c => c.Id == verifyCustomerRequestDto.customerId);

                if (custDetails is null) return ExecutedResult<VerifyCustomerResponseDto>.Failed("Customer does not exists");


                var result = await _oTPService.ValidateOTP(verifyCustomerRequestDto.phoneNumber, verifyCustomerRequestDto.otp);

                if (result.Response != ResponseCode.Ok) return ExecutedResult<VerifyCustomerResponseDto>.Failed("Could not validate OTP");

                custDetails.IsAccountActive = true;

                var response = new VerifyCustomerResponseDto()
                {
                    email = custDetails.Email,
                    phoneNumber = custDetails.PhoneNumber
                };


                return ExecutedResult<VerifyCustomerResponseDto>.Success(response, "Customer details verified successfully.");
            }
            catch (Exception)
            {
                return ExecutedResult<VerifyCustomerResponseDto>.Exception("Something went wrong");
            }
        }

        private Tuple<string, string> GetStateLga(long lgaId)
        {
            var result1 = _dbLga.FirstOrDefault(s => s.Id == lgaId);
            var result2 = _dbStates.FirstOrDefault(s => s.Id == result1.StateId);
            return new Tuple<string, string>(result2.Name, result1.Name);
        }
    }
}
