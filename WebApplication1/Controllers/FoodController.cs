using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class FoodController : Controller {
        //getAll
        [HttpGet]
        public IActionResult GetAll() {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                return Ok(FoodsServices.GetAll());
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        //getSingle
        [HttpGet("{id:long}")]
        public IActionResult GetSingle(long id) {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                Food? item = FoodsServices.GetSingle(id);
                if (item == null) return NotFound(null);
                return Ok(item);

            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        //create
        [HttpPost]
        public IActionResult AddItem(Food newItem) {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                return Created(nameof(AddItem),FoodsServices.Create(newItem));
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        //edit
        [HttpPatch("{id:long}")]
        public IActionResult EditItem(long id,[FromBody] Food item) {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                Food? itemEdited = FoodsServices.Edit(id,item);
                if (itemEdited == null) return NotFound(null);
                return Ok(itemEdited);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        //delete
        [HttpDelete("{id:long}")]
        public IActionResult DeleteItem(long id) {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                int changes = FoodsServices.Delete(id);
                if (changes <= 0) return NotFound(null);
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
