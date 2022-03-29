using Customer.API.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Customer.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class LgaController : BaseController
    {
        private readonly ILgaService _LgaService;

        public LgaController(ILgaService LgaService)
        {
            _LgaService = LgaService;
        }

        [HttpGet(Name = "GetLga")]
        public async Task<IActionResult> GetLga([FromQuery] long stateId)
        {
            var response = await _LgaService.Lgas(stateId);
            return CustomResponse(response);
        }
    }
}
