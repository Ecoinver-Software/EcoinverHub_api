using EcoinverHub_api.Data;
using EcoinverHub_api.Models;
using EcoinverHub_api.Models.Dto.Create;
using EcoinverHub_api.Models.Dto.Update;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoinverHub_api.Models.Identity;
//using EcoinverHub_api.Migrations;

namespace EcoinverHub_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnunciosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnunciosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Anuncios
        [HttpGet]
        public async Task<IActionResult> GetAnuncios()
        {
            // Retornamos solo los que no estén eliminados (“IsDeleted == false”).
            var lista = await _context.Set<Anuncio>()
                                     .Where(a => !a.IsDeleted)
                                     .Select(a => new
                                     {
                                         id=a.Id,
                                         nombre=a.nombre,
                                         estado=a.estado,
                                         contenido=a.contenido
                                     }
                                     )
                                     .ToListAsync();
            return Ok(lista);
        }

        // GET: api/Anuncios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnuncioPorId([FromRoute] int id)
        {
            var anuncio = await _context.Set<Anuncio>()
                                        .Where(a => !a.IsDeleted)
                                     .Select(a => new
                                     {
                                         id = a.Id,
                                         nombre = a.nombre,
                                         estado = a.estado,
                                         contenido = a.contenido
                                     }
                                     )
                                     .ToListAsync();

            if (anuncio == null)
                return NotFound(new { Message = "No se ha encontrado el anuncio con el id especificado." });

            return Ok(anuncio);
        }

        // POST: api/Anuncios
        [HttpPost]
        public async Task<IActionResult> CrearAnuncio([FromBody] CreateAnuncioDto dto)
        {
            // Validar que el DTO no venga vacío en sus campos requeridos
            if (string.IsNullOrWhiteSpace(dto.nombre) ||
                string.IsNullOrWhiteSpace(dto.estado) ||
                string.IsNullOrWhiteSpace(dto.contenido))
            {
                return BadRequest(new { Message = "Debe proporcionar nombre, estado y contenido del anuncio." });
            }

            var nuevo = new Anuncio
            {
                nombre = dto.nombre.Trim(),
                estado = dto.estado.Trim(),
                contenido = dto.contenido.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
                // deletedAt queda en null por defecto en BaseModel
            };

            _context.Add(nuevo);
            await _context.SaveChangesAsync();

            // Opcional: devolver el objeto recién creado
            return Ok(new
            {
                id = nuevo.Id,
                nombre = nuevo.nombre,
                estado = nuevo.estado,
                contenido = nuevo.contenido,
                createdAt = nuevo.CreatedAt,
                updatedAt = nuevo.UpdatedAt
            });
        }

        // PUT: api/Anuncios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnuncio(int id, [FromBody] UpdateAnuncioDto dto)
        {
            var anuncio = await _context.Set<Anuncio>().FindAsync(id);
            if (anuncio == null || anuncio.IsDeleted)
                return NotFound(new { Message = "No se ha encontrado el anuncio con el id especificado." });

            // Solo modificamos los campos que vengan “no nulos o no vacíos”
            if (!string.IsNullOrWhiteSpace(dto.nombre))
                anuncio.nombre = dto.nombre.Trim();

            if (!string.IsNullOrWhiteSpace(dto.estado))
                anuncio.estado = dto.estado.Trim();

            if (!string.IsNullOrWhiteSpace(dto.contenido))
                anuncio.contenido = dto.contenido.Trim();

            anuncio.UpdatedAt = DateTime.UtcNow;

            _context.Update(anuncio);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Anuncio actualizado correctamente." });
        }

        // DELETE: api/Anuncios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAnuncio(int id)
        {
            var anuncio = await _context.Set<Anuncio>().FindAsync(id);
            if (anuncio == null || anuncio.IsDeleted)
                return NotFound(new { Message = "No se ha encontrado el anuncio con el id especificado." });

            // “Borrado lógico”:
            anuncio.IsDeleted = true;
            anuncio.DeletedAt = DateTime.UtcNow;
            anuncio.UpdatedAt = DateTime.UtcNow;

            _context.Update(anuncio);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Anuncio eliminado (marcado como borrado) correctamente." });
        }
    }
}
