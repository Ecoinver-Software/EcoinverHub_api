using EcoinverHub_api.Models;
using EcoinverHub_api.Models.Dto.Create;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoinverHub_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        public readonly RoleManager<ApplicationRole> _roleManager;

        public RolesController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var todosLosRoles = await _roleManager.Roles.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.Level,
                x.NormalizedName,


            }).ToListAsync();
            // IQueryable<ApplicationRole>
            return Ok(todosLosRoles);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {

            var resultado = await _roleManager.CreateAsync(new ApplicationRole
            {
                Name = dto.Name,
                Description = dto.Description,
                Level = dto.Level,
                NormalizedName = dto.Name.ToUpper()

            });
            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            var envio = await _roleManager.Roles
            .Where(x => x.Name == dto.Name)
            .FirstOrDefaultAsync();

            return Ok(envio);

        }
        [HttpPut("{id}")]
           public async Task<IActionResult> Actualizar(int id, [FromBody] CreateRoleDto dto)
        {
            var rol = await _roleManager.FindByIdAsync(id.ToString());
            if (rol==null)
            {
                return NotFound(new {message="No se ha encontrado el rol"});
            }
            rol.Name = dto.Name;
            rol.Level = dto.Level;
            rol.Description = dto.Description;

            var envio = await _roleManager.Roles
          .Where(x => x.Name == dto.Name)
          .FirstOrDefaultAsync();
            await _roleManager.UpdateAsync(rol);

            return Ok(rol);

        }
            

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rol = await _roleManager.FindByIdAsync(id.ToString());

            if (rol==null)
            {
                return NotFound(new {message="No existe el rol con el id especificado"});
            }

            var result = await _roleManager.DeleteAsync(rol);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new {message="Rol eliminado correctamente"});
        }

    }
}
