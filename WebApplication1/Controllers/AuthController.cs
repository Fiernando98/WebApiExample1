using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class AuthController : Controller {
        [HttpGet("login/")]
        public IActionResult GetAll() {
            try {
                string? basicAuth = Request.Headers["Authorization"];
                if (basicAuth == null) throw new HttpResponseException(statusCode: StatusCodes.Status401Unauthorized);

                byte[] bytes = Convert.FromBase64String(basicAuth.Split("Basic ").LastOrDefault()!);
                string[] decodedString = Encoding.UTF8.GetString(bytes).Split(":");
                string username = decodedString.FirstOrDefault()!;
                string password = decodedString.LastOrDefault()!;

                UserRegistrer? userRegistrer = AuthServices.GetUserRegistrer(username);
                if (userRegistrer == null) throw new HttpResponseException(statusCode: StatusCodes.Status404NotFound,error: "Cuenta no encontrada");
                if (!password.Equals(EncryptionServices.Decrypt(userRegistrer.Password!,userRegistrer.EncryptGUID!)))
                    throw new HttpResponseException(statusCode: StatusCodes.Status401Unauthorized,error: "Contraseña inválida");
                return Ok(AuthServices.Create(new AuthToken {
                    Token = Guid.NewGuid().ToString(),
                    User = userRegistrer.getUser()
                }));
            } catch (HttpResponseException httpError) {
                return StatusCode(httpError.StatusCode,httpError.Error);
            }
        }
    }
}
