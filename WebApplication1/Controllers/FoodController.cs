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
                return Ok(FoodsServices.GetAll(new WhereSQL {
                    SQLClauses = new string[] {
                    }
                }));
            } catch (HttpResponseException httpError) {
                return StatusCode(httpError.StatusCode,httpError.Error);
            }
        }

        //getSingle
        [HttpGet("{id:long}")]
        public IActionResult GetSingle(long id) {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                Food? item = FoodsServices.GetSingle(new WhereSQL {
                    SQLClauses = new string[] {
                        $"{FoodSQLTable.id} = {id}"
                    }
                });
                if (item == null) return NotFound(null);
                return Ok(item);
            } catch (HttpResponseException httpError) {
                return StatusCode(httpError.StatusCode,httpError.Error);
            }
        }

        //create
        [HttpPost]
        public IActionResult AddItem(Food newItem) {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                return Created(nameof(AddItem),FoodsServices.Create(newItem));
            } catch (HttpResponseException httpError) {
                return StatusCode(httpError.StatusCode,httpError.Error);
            }
        }

        //edit
        [HttpPatch("{id:long}")]
        public IActionResult EditItem(long id,[FromBody] Food item) {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                item.ID = id;
                Food itemEdited = FoodsServices.Edit(new WhereSQL {
                    SQLClauses = new string[] {
                        $"{FoodSQLTable.id} = {id}"
                    }
                },item);
                return Ok(itemEdited);
            } catch (HttpResponseException httpError) {
                return StatusCode(httpError.StatusCode,httpError.Error);
            }
        }

        //delete
        [HttpDelete("{id:long}")]
        public IActionResult DeleteItem(long id) {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                bool isSuccessfully = FoodsServices.Delete(new WhereSQL {
                    SQLClauses = new string[] {
                        $"{FoodSQLTable.id} = {id}"
                    }
                });
                if (!isSuccessfully) return NotFound(null);
                return NoContent();
            } catch (HttpResponseException httpError) {
                return StatusCode(httpError.StatusCode,httpError.Error);
            }
        }
    }
}
