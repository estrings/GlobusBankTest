using Customer.API.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Customer.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class StateController : BaseController
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [HttpPost(Name = "CreateState")]
        public async Task<IActionResult> CreateState([FromBody] string name)
        {
            var response = await _stateService.Create(name);
            return CustomResponse(response);
        }

        [HttpGet(Name = "GetState")]
        public async Task<IActionResult> GetState([FromQuery] long id)
        {
            var response = await _stateService.GetState(id);
            return CustomResponse(response);
        }

        [HttpGet(Name = "States")]
        public async Task<IActionResult> States()
        {
            var response = await _stateService.States();
            return CustomResponse(response);
        }
    }
}
