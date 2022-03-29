using Customer.API.Controllers;
using Customer.API.Service.Interfaces;
using Customer.XUnitTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Customer.XUnitTest.Controller
{
    public class StateControllerTest
    {
        private readonly StateController _controller;
        private readonly IStateService _stateService;

        public StateControllerTest()
        {
            _stateService = new StateServiceTest();
            _controller = new StateController(_stateService);            
        }

        [Fact]
        public async Task Get_AllStates()
        {
            //Act
            var okResult = await _controller.States();

            //Assert 
            Assert.IsType<OkObjectResult>(okResult);
        }

       [Fact]
       public async Task GetById_UnknownIdPassed()
        {
            //Act
            var notFoundResult = await _controller.GetState(4);

            //Assert 
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public async Task GetById_KnownIdPassed()
        {
            //Act
            var oKResult = await _controller.GetState(1);

            //Assert 
            Assert.IsType<OkObjectResult>(oKResult);
        }

        [Fact]
        public async Task Add_CreateStateObjectPassed()
        {
            // Arrange
            var payload = "Benue";
            // Act
            var createdResponse = await _controller.CreateState(payload);
            // Assert
            Assert.IsType<OkObjectResult>(createdResponse);
        }
    }
}
