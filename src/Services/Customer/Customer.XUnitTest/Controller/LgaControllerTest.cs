using Customer.API.Controllers;
using Customer.API.Service.Interfaces;
using Customer.XUnitTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Customer.XUnitTest.Controller
{
    public class LgaControllerTest
    {
        private readonly LgaController _controller;
        private readonly ILgaService _lgaService;

        public LgaControllerTest()
        {
            _lgaService = new LgaServiceTest();
            _controller = new LgaController(_lgaService);           
        }

        [Fact]
        public async Task GetById_UnknownStateIdPassed()
        {
            //Act
            var notFoundResult = await _controller.GetLga(4);

            //Assert 
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public async Task GetById_KnownStateIdPassed()
        {
            //Act
            var oKResult = await _controller.GetLga(1);

            //Assert 
            Assert.IsType<OkObjectResult>(oKResult);
        }
    }
}
