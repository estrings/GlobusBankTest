using Customer.API.Common.ResourceParameters;
using Customer.API.Controllers;
using Customer.API.Service.Dtos.Request;
using Customer.API.Service.Interfaces;
using Customer.XUnitTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Customer.XUnitTest.Controller
{
    public class CustomerControllerTest
    {
        private readonly CustomerController _controller;
        private readonly ICustomerService _customerService;

        public CustomerControllerTest()
        {
            _customerService = new CustomerServiceTest();
            _controller = new CustomerController(_customerService);
        }

        [Fact]
        public async Task Get_AllCustomers()
        {
            //Arrage 
            ResourceParameters resourceParameters = new ResourceParameters()
            {
                PageSize = 1,
                PageNumber =  10
            };
            //Act
            var okResult = await _controller.Customers(resourceParameters);

            //Assert 
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task Get_NoCustomersFound()
        {
            //Arrage 
            ResourceParameters resourceParameters = new ResourceParameters()
            {
                PageSize = 1,
                PageNumber = 10
            };
            //Act
            var okResult = await _controller.Customers(resourceParameters);

            //Assert 
            Assert.IsType<NotFoundObjectResult>(okResult);
        }

        [Fact]
        public async Task IsCustomers_NotValidOTP()
        {
            //Arrange
            VerifyCustomerRequestDto verifyCustomerRequestDto = new VerifyCustomerRequestDto()
            {
                customerId = 1,
                otp = "23456",
                phoneNumber = "08167970254"
            };
            var okResult = await _controller.VerifyCustomer(verifyCustomerRequestDto);
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        [Fact]
        public async Task IsCustomers_ValidOTP()
        {
            //Arrange
            VerifyCustomerRequestDto verifyCustomerRequestDto = new VerifyCustomerRequestDto()
            {
                customerId = 1,
                otp = "12345",
                phoneNumber = "08167970254"
            };
            var okResult = await _controller.VerifyCustomer(verifyCustomerRequestDto);
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task IsCustomers_InValidCustomerId()
        {
            //Arrange
            VerifyCustomerRequestDto verifyCustomerRequestDto = new VerifyCustomerRequestDto()
            {
                customerId = 4,
                otp = "23456",
                phoneNumber = "08167970254"
            };
            //Act
            var okResult = await _controller.VerifyCustomer(verifyCustomerRequestDto);
            //Assert
            Assert.IsType<BadRequestObjectResult>(okResult);
        }

        //[Fact]
        //public async Task IsCustomers_ValidateCustomer()
        //{
        //    //Arrange
        //    VerifyCustomerRequestDto verifyCustomerRequestDto = new VerifyCustomerRequestDto()
        //    {
        //        customerId = 1,
        //        otp = "23456",
        //        phoneNumber = "08167970254"
        //    };
        //    //Act
        //    var okResult = await _controller.VerifyCustomer(verifyCustomerRequestDto);
        //    //Assert
        //    Assert.IsType<OkObjectResult>(okResult);
        //}

        [Fact]
        public async Task IsCustomers_CustomerExist()
        {
            //Arrange
            CustomerRequestDto customerRequestDto = new CustomerRequestDto()
            {
                email = "trustlead01@gmail.com",
                lga = 1,
                phoneNumber = "08167970254",
                password = "true",
                state = 1
            };
            //Act
            var okResult = await _controller.CreateCustomer(customerRequestDto);
            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task IsCustomers_CustomerNotExist()
        {
            //Arrange
            CustomerRequestDto customerRequestDto = new CustomerRequestDto()
            {
                email = "ejeguoe@gmail.com",
                lga = 1,
                phoneNumber = "08163970254",
                password = "true",
                state = 1
            };
            //Act
            var okResult = await _controller.CreateCustomer(customerRequestDto);
            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }
    }
}
