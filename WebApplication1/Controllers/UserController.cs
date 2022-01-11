using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class UserController : Controller {
        //create
        [HttpPost]
        public IActionResult AddItem(UserRegistrer newItem) {
            try {
                return Created(nameof(AddItem),UsersServices.Create(newItem));
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
