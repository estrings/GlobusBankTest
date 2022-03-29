using Customer.API.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Customer.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class GoldController : BaseController
    {
        private readonly IGoldService _goldService;
        public GoldController(IGoldService goldService)
        {
            _goldService = goldService;
        }

        [HttpGet(Name = "GetPrices")]
        public async Task<IActionResult> GetPrices()
        {
            var response = await _goldService.GoldPrice();
            return CustomResponse(response);
        }
    }
}
