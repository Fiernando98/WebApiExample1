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
    }
}
