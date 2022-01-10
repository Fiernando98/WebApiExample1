using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class UserController : Controller {
        //create
        [HttpPost]
        public IActionResult AddItem(User newItem) {
            try {
                return Created(nameof(AddItem), UserSQLTable.toCreateQuery);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
