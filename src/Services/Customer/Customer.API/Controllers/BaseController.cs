using Customer.API.Common.Communication;
using Customer.API.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult CustomResponse<T>(ExecutedResult<T> result)
        {
            switch (result.Response)
            {
                case ResponseCode.NotFound:
                    return NotFound(result);
                case ResponseCode.ProcessingError:
                    return BadRequest(result);
                case ResponseCode.AuthorizationError:
                    return Unauthorized(result);
                case ResponseCode.Exception:
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
                default:
                    return Ok(result);
            }
        }
    }
}
