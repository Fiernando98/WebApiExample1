using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class RestaurantController : ControllerBase {
        //getAll
        [HttpGet]
        public IActionResult GetAll() {
            try {
                return Ok(RestaurantsServices.GetAll());
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        //getSingle
        [HttpGet("{id:long}")]
        public IActionResult GetSingle(long id) {
            try {
                Restaurant? item = RestaurantsServices.GetSingle(id);
                if (item != null) return Ok(item);
                else return NotFound(null);

            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        //create
        [HttpPost]
        public IActionResult AddItem(Restaurant newItem) {
            try {
                return Created(nameof(AddItem),RestaurantsServices.Create(newItem));
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        //edit
        [HttpPatch("{id:long}")]
        public IActionResult EditItem(long id,[FromBody] Restaurant item) {
            try {
                Restaurant? itemEdited = RestaurantsServices.Edit(id,item);
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
                int changes = RestaurantsServices.Delete(id);
                if (changes <= 0) return NotFound(null);
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
