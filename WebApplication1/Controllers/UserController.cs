using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class UserController : Controller {
        //create
        [HttpPost]
        public IActionResult AddItem(UserLogin newItem) {
            try {
                return Created(nameof(AddItem),UsersServices.Create(newItem));
            } catch (HttpResponseException httpError) {
                return StatusCode(httpError.StatusCode,httpError.Error);
            }
        }

        //change password
        [HttpPost("change_password/")]
        public IActionResult ChangePassword(ChangePassword changePassword) {
            try {
                string? token = Request.Headers["Authorization"].ToString().Split(" ").LastOrDefault();
                if (!AuthServices.ValidateToken(token))
                    return Unauthorized("Credenciales inválidas");
                UserRegistrer? userRegistrer = AuthServices.GetUserRegistrerByToken(token);
                if (userRegistrer == null) return NotFound("Usuario no encontrado");
                if (!changePassword.CurrentPassword!.Equals(EncryptionServices.Decrypt(userRegistrer.Password!,userRegistrer.EncryptGUID!)))
                    return Unauthorized("Contraseña inválida");
                UsersServices.ChangePassword(whereSQL: new WhereSQL {
                    SQLClauses = new string[] {
                        $"{UserSQLTable.id} = {userRegistrer.ID}"
                    }
                },newPassword: changePassword.NewPassword);
                return NoContent();
            } catch (HttpResponseException httpError) {
                return StatusCode(httpError.StatusCode,httpError.Error);
            }
        }
    }
}
