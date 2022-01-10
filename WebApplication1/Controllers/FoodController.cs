using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class FoodController : ControllerBase {
        //getAll
        [HttpGet]
        public IActionResult GetAll() {
            try {
                return Ok(FoodsServices.GetAll());
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //getSingle
        [HttpGet("{id}")]
        public IActionResult GetSingle(int id) {
            try {
                Food? item = FoodsServices.GetSingle(id);
                if (item != null) return Ok(item);
                else return NotFound(null);

            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //create
        [HttpPost]
        public IActionResult AddItem(Food newItem) {
            try {
                return Created(nameof(AddItem), FoodsServices.Create(newItem));
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //edit
        [HttpPatch("{id}")]
        public IActionResult EditItem(int id, [FromBody] Food item) {
            try {
                return Ok();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //delete
        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id) {
            try {
                Food? item = FoodsServices.GetSingle(id);
                if (item == null) return NotFound(null);
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
