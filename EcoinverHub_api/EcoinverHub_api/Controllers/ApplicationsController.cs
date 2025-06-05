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
        public async Task<IActionResult> Crear([FromBody] CreateAplicationDto dto)
        {
            var aplicacion = new Application
            {
                Name = dto.Name,
                Description = dto.Description,
                Url = dto.Url,
                Icon = dto.Icon
            };


            _context.AddAsync(aplicacion);
            await _context.SaveChangesAsync();

            return Ok(aplicacion);
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
