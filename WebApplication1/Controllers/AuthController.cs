﻿using Microsoft.AspNetCore.Mvc;
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
                if (basicAuth == null) return Unauthorized(null);

                byte[] bytes = Convert.FromBase64String(basicAuth.Split("Basic ").LastOrDefault()!);
                string[] decodedString = Encoding.UTF8.GetString(bytes).Split(":");
                string username = decodedString.FirstOrDefault()!;
                string password = decodedString.LastOrDefault()!;

                UserRegistrer? userRegistrer = AuthServices.GetUserRegistrer(username);
                if (userRegistrer == null) return NotFound("Cuenta no encontrada");
                if (!password.Equals(EncryptionServices.Decrypt(userRegistrer.Password!,userRegistrer.EncryptGUID!)))
                    return Unauthorized("Contraseña invalida");
                return Ok(AuthServices.Create(new AuthToken {
                    Token = Guid.NewGuid().ToString(),
                    User = userRegistrer.getUser()
                }));
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
