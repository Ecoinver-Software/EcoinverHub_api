using System.Globalization;
using EcoinverHub_api.Data;
using EcoinverHub_api.Models;
using EcoinverHub_api.Models.Dto.Create;
using EcoinverHub_api.Models.Dto.Update;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoinverHub_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ApplicationsController(AppDbContext context) {

            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Get()
        {
            var aplicaciones = await _context.Applications.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.Icon,
                x.Url,
            }).ToListAsync();

            return Ok(aplicaciones);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var aplicacion = await _context.Applications.FindAsync(id);
            if (aplicacion == null)
            {
                return NotFound(new { message = "No se ha encontrado la aplicación el id especificado" });
            }
            return Ok(aplicacion);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(IFormFile image, [FromForm] string name, [FromForm] string description, [FromForm] string url)
        {
            if (image==null || image.Length==0)
            {
                return BadRequest(new { message = "No se ha encontrado ninguna imgen" });

            }
            var uploadsFolder = Path.Combine("wwwroot", "uploads");//Comprobamos que la carpeta exista.
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var extension = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extensionImagen = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!extension.Contains(extensionImagen))//Si no contiene una extensión válida devolvemos un BadRequest.
            {
                return BadRequest(new { message = "La extensión de la imgen no es válida" });
            }
            var filePath = Path.Combine("wwwroot/uploads", image.FileName);
            var rutaBaseDatos = Path.Combine("uploads", image.FileName);
            using(var stream=new FileStream(filePath,FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }


            var aplicacion = new Application
            {
                Name = name,
                Description = description,
                Url = url,
                Icon = rutaBaseDatos
            };
            _context.Applications.Add(aplicacion);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Se ha guardado correctamente la aplicación" });

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Editar([FromRoute] int id, [FromBody] UpdateApplicationDto dto)
        {
            var aplicacion = await _context.Applications.FindAsync(id);

            if (aplicacion==null)
            {
                return NotFound(new {message="No se ha encontrado la aplicacion el id especificado"});
            }
            aplicacion.Name = dto.Name;
            aplicacion.Url = dto.Url;
            aplicacion.Icon = dto.Icon;
            aplicacion.Description = dto.Description;

            await _context.SaveChangesAsync();

            return Ok(aplicacion);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Borrar(int id)
        {
            var aplicacion=await _context.Applications.FindAsync(id);
            if (aplicacion == null)
            {
                return NotFound(new {message="No se ha encontrado el id especificado"});

            }
            _context.Remove(aplicacion);
            await _context.SaveChangesAsync();
            return Ok(aplicacion);
        }
    }
}
