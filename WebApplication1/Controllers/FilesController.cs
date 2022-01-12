using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using WebApplication1.Models;
using System.Text;

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
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                return Ok(FilesServices.Create(file,$@"{_env.ContentRootPath}files/").Result);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll() {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                return Ok(FilesServices.GetAll(new WhereSQL {
                    SQLClauses = new string[] {
                    }
                }));
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpGet("{id:long}")]
        public IActionResult Get(long id) {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                Files? item = FilesServices.GetSingle(new WhereSQL {
                    SQLClauses = new string[] {
                        $"{FilesSQLTable.id} = {id}"
                    }
                });
                if (item == null) return NotFound(null);
                return Ok(item);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpGet("content/{id:long}")]
        public IActionResult GetContent(long id) {
            try {
                if (!AuthServices.ValidateToken(Request.Headers["Authorization"]))
                    return Unauthorized("Credenciales inválidas");
                Files? item = FilesServices.GetSingle(new WhereSQL {
                    SQLClauses = new string[] {
                        $"{FilesSQLTable.id} = {id}"
                    }
                });
                if (item == null) return NotFound(null);
                if (!System.IO.File.Exists(item.Path)) {
                    if (item == null) return NotFound(null);
                }

                return File(FilesServices.GetFileBytes(item!.Path!).Result,FilesServices.GetMimeType(item?.Path?.Split(".")?.LastOrDefault()));
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
                bool isSuccessfully = FilesServices.Delete(new WhereSQL {
                    SQLClauses = new string[] {
                        $"{FilesSQLTable.id} = {id}"
                    }
                });
                if (!isSuccessfully) return NotFound(null);
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
