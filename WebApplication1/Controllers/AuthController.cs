using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class AuthController : ControllerBase {
        [HttpGet("login/")]
        public IActionResult GetAll() {
            try {
                /*string? bearerToken = Request.Headers["Authorization"];
                return Ok(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
                if(bearerToken == null) return Unauthorized(null);*/
                return Ok(Request.Headers["Authorization"]);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
