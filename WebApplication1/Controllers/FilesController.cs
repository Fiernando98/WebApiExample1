using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class FilesController : Controller {
        //create
        [HttpPost]
        public IActionResult AddItem(IFormFile file) {
            try {
                return Ok(file);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
