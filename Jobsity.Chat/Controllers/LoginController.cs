using Jobsity.Chat.Domain.Dtos.Login;
using Jobsity.Chat.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Jobsity.Chat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Login([FromBody] LoginDto loginDto, [FromServices] ILoginService services)
        {
            if (loginDto == null)
            {
                return BadRequest();
            }

            try
            {
                var result = await services.Authenticate(loginDto);

                if (result != null)
                {
                    return result;
                }

                return BadRequest("Wrong email or password.");
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
