using Customer.API.Controllers;
using Customer.API.Service.Interfaces;
using Customer.XUnitTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Customer.XUnitTest.Controller
{
    public class GoldControllerTest
    {
        private readonly GoldController _controller;
        private readonly IGoldService _goldService;

        public GoldControllerTest()
        {
            _goldService = new GoldServiceTest();
            _controller = new GoldController(_goldService);
        }

        [Fact]
        public async Task Get_AllGold()
        {
            //Act
            var okResult = await _controller.GetPrices();

            //Assert 
            Assert.IsType<OkObjectResult>(okResult);
        }
    }
}
