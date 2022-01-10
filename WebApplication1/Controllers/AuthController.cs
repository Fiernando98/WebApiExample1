using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class AuthController : ControllerBase {
        [HttpGet("login/")]
        public IActionResult GetAll() {
            try {
                string? basicAuth = Request.Headers["Authorization"];
                if (basicAuth == null) return Unauthorized(null);

                byte[] bytes = Convert.FromBase64String(basicAuth.Split("Basic ").LastOrDefault()!);
                string[] decodedString = Encoding.UTF8.GetString(bytes).Split(":");
                string username = decodedString.FirstOrDefault()!;
                string password = decodedString.LastOrDefault()!;

                Guid tokenID = Guid.NewGuid();

                return Ok(new AuthToken {
                    Token = tokenID.ToString()
                });
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
