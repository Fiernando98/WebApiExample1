using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class FilesController : Controller {
        private readonly IWebHostEnvironment _env;

        public FilesController(IWebHostEnvironment env) {
            _env = env;
        }
        //create
        [HttpPost]
        public IActionResult AddItem(IFormFile file) {
            try {
                return Ok(FilesServices.Create(file,$@"{_env.ContentRootPath}files/").Result);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
