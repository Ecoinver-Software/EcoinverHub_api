using EcoinverHub_api.Data;
using EcoinverHub_api.Models;
using EcoinverHub_api.Models.Dto.Create;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoinverHub_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleApplicationController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RoleApplicationController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rolesAplicaciones = await _context.RoleApplications.Select(x => new
            {
                x.Id,
                x.UserId,
                x.ApplicationId
            }).ToListAsync();

            return Ok(rolesAplicaciones);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CreateRoleApplications dto)
        {
            var rolAplicacion = new RoleApplication
            {
                UserId = dto.UserId,
                ApplicationId = dto.ApplicationId
            };
            var busqueda = await _context.RoleApplications.Where(x => x.UserId == dto.UserId && x.ApplicationId == dto.ApplicationId).ToListAsync();
            if (busqueda.Any())
            {
                return BadRequest(new { message = "La aplicación ya se encuentra asignada a este usuario" });
            }

            _context.RoleApplications.Add(rolAplicacion);
            await _context.SaveChangesAsync();

            return Ok(rolAplicacion);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rolAplicacion = await _context.RoleApplications.FindAsync(id);
            if (rolAplicacion==null)
            {
                return NotFound(new { message = "No se ha encontrado el id del rol de aplicación asignado" });
            }

            _context.RoleApplications.Remove(rolAplicacion);

            _context.SaveChangesAsync();
            return Ok(new {message="Se ha eliminado el rol de aplicación correctamente"});
        }
    }
}
