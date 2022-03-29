using Customer.API.Common.ResourceParameters;
using Customer.API.Service.Dtos.Request;
using Customer.API.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Customer.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost(Name = "CreateCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequestDto customerRequestDto)
        {
            var response = await _customerService.CreateCustomer(customerRequestDto);
            return CustomResponse(response);
        }

        [HttpPost(Name = "VerifyCustomer")]
        public async Task<IActionResult> VerifyCustomer([FromBody] VerifyCustomerRequestDto verifyCustomerRequestDto)
        {
            var response = await _customerService.VerifyCustomer(verifyCustomerRequestDto);
            return CustomResponse(response);
        }

        [HttpGet(Name = "Customers")]
        public async Task<IActionResult> Customers([FromQuery] ResourceParameters resourceParameters)
        {
            var response = await _customerService.Customers(resourceParameters);
            return CustomResponse(response);
        }
    }
}
