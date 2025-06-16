using EcoinverHub_api.Data;
using EcoinverHub_api.Models;
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
                x.Autor,
                x.Estado,
                x.FechaActualizacion,
                x.Version
               
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
        public async Task<IActionResult> Crear(IFormFile image, [FromForm] string name, [FromForm] string description, [FromForm] string url, [FromForm] string estado, [FromForm] string version, [FromForm] string autor)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(new { message = "No se ha encontrado ninguna imagen" });
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Console.WriteLine($"Directorio actual: {Directory.GetCurrentDirectory()}");
            Console.WriteLine($"Carpeta uploads: {uploadsFolder}");
            Console.WriteLine($"¿Existe el directorio? {Directory.Exists(uploadsFolder)}");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
                Console.WriteLine("Directorio creado");
            }

            var extension = new[]
            {
        ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".tiff",
        ".ico", ".svg", ".heif", ".heic", ".raw", ".exr", ".avif", ".dng"
    };

            var extensionImagen = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!extension.Contains(extensionImagen))
            {
                return BadRequest(new { message = "La extensión de la imagen no es válida" });
            }

            var filePath = Path.Combine(uploadsFolder, image.FileName);
            var rutaBaseDatos = Path.Combine("uploads", image.FileName);

            Console.WriteLine($"Ruta completa del archivo: {filePath}");

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                Console.WriteLine("Archivo guardado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar archivo: {ex.Message}");
                return BadRequest(new { message = $"Error al guardar archivo: {ex.Message}" });
            }

            var aplicacion = new Application
            {
                Name = name,
                Description = description,
                Url = url,
                Icon = rutaBaseDatos,
                Estado = estado,
                Version = version,
                Autor = autor,
                FechaActualizacion = DateTime.Now
            };

            _context.Applications.Add(aplicacion);
            await _context.SaveChangesAsync();
            return Ok(aplicacion);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Editar([FromRoute] int id, IFormFile? image, [FromForm] string name, [FromForm] string description, [FromForm] string url, [FromForm] string estado, [FromForm] string version, [FromForm] string autor)
        {
            var aplicacion = await _context.Applications.FindAsync(id);

            

            if (aplicacion == null)
            {
                return NotFound(new { message = "No se ha encontrado la aplicacion el id especificado" });
            }
            aplicacion.Name = name;
            aplicacion.Description = description;
            aplicacion.Url = url;
            aplicacion.Estado = estado;
            aplicacion.Version = version;
            aplicacion.Autor = autor;
            aplicacion.FechaActualizacion = DateTime.Now;
            if (image != null) // Verificamos si se envió una imagen.
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var extension = new[]
{
    ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".tiff",
    ".ico", ".svg", ".heif", ".heic", ".raw", ".exr", ".avif", ".dng"
};
                var extensionImagen = Path.GetExtension(image.FileName).ToLowerInvariant();

                if (!extension.Contains(extensionImagen)) // Si no contiene una extensión válida, devolvemos un BadRequest.
                {
                    return BadRequest(new { message = "La extensión de la imagen no es válida" });
                }

                var filePath = Path.Combine("wwwroot/uploads", image.FileName);
                var rutaBaseDatos = Path.Combine("uploads", image.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                aplicacion.Icon = rutaBaseDatos;
            }

            await _context.SaveChangesAsync();

            
           

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
