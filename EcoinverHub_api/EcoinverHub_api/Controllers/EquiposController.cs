using EcoinverHub_api.Data;
using EcoinverHub_api.Models;
using EcoinverHub_api.Models.Dto.Create;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoinverHub_api.Controllers
{
    [Controller]
    [Route("api/[controller]")]//Para las rutas de la api
    public class EquiposController:ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public EquiposController(AppDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
            var equipos = await _context.Equipos.Include(x=>x.JefeEquipo).Select(x => new//El include relaciona automaticamente las tablascon la clave foranea
            {
                x.Id,
                x.Nombre,
                NombreJefe=x.JefeEquipo.name,
                x.Empresa
            }).ToListAsync();
            return Ok(equipos);
        }
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CreateEquipo dto)
        {
            var Equipos = new Equipos
            {
                Nombre = dto.Nombre,
                Empresa = dto.Empresa,
                JefeEquipoId = dto.JefeEquipoId
            };
            
            _context.Equipos.Add(Equipos);
            await _context.SaveChangesAsync();
            var equipo = await _context.Equipos
    .Include(x => x.JefeEquipo)
    .Where(x => x.Id == Equipos.Id)
    .Select(x => new {
        x.Id,
        x.Nombre,
        NombreJefe = x.JefeEquipo.name,
        x.Empresa
    })
    .FirstOrDefaultAsync(); // <-- devuelve un solo objeto


            return Ok(equipo);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var equipos=await _context.Equipos.FindAsync(id);

            if (equipos==null)
            {
                return NotFound(new {message="No se ha encontrado el equipo con el ID especificado"});
            }
                

            return Ok(equipos);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar([FromRoute] int id, [FromBody] CreateEquipo dto)
        {
            var equipos = await _context.Equipos.FindAsync(id);
            if (equipos == null)
            {
                return NotFound(new { message = "No se ha encontrado el equipo con el ID especificado" });
            }
            equipos.Nombre = dto.Nombre;
            equipos.Empresa = dto.Empresa;
            equipos.JefeEquipoId = dto.JefeEquipoId;
            _context.Equipos.Update(equipos);
            await _context.SaveChangesAsync();
            await _context.SaveChangesAsync();
            var equipo = await _context.Equipos
            .Include(x => x.JefeEquipo)
            .Where(x => x.Id == equipos.Id)
            .Select(x => new {
            x.Id,
            x.Nombre,
            NombreJefe = x.JefeEquipo.name,
            x.Empresa
             })
            .FirstOrDefaultAsync(); // <-- devuelve un solo objeto

            return Ok(equipo);
        }
        
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var equipo=await _context.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return NotFound(new { message = "No se ha encontrado el equipo con el ID especificado" });
            }
            _context.Equipos.Remove(equipo);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Equipo eliminado correctamente" });
        }
    }
}
