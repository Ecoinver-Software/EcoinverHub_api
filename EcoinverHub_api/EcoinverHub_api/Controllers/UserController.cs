using EcoinverHub_api.Data;
using EcoinverHub_api.Models;
using EcoinverHub_api.Models.Dto.Create;
using EcoinverHub_api.Models.Dto.Update;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoinverHub_api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public UserController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            // Traemos los usuarios incluyendo el rol para evitar muchas consultas
            var usuarios = await _context.Users.Include(u => u.Role).ToListAsync();

            var listaUsuarios = usuarios.Select(user => new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.name,
                user.lastname,
                user.Empresa,
                user.EquipoId,

                Roles = user.Role.Name // Lista con un solo rol, o vacía si null
            });

            return Ok(listaUsuarios);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioPorId([FromRoute] int id)
        {
            // 1) Buscamos el usuario por Id, incluyendo la entidad Role
            var usuario = await _context.Users
                                .Include(u => u.Role)
                                .Where(u => u.Id == id)
                                .Select(u => new
                                {
                                    u.Id,
                                    u.UserName,
                                    u.Email,
                                    u.name,
                                    u.lastname,
                                    u.Empresa,
                                    u.EquipoId,
                                    Roles = u.Role != null ? u.Role.Name : null
                                })
                                .FirstOrDefaultAsync();

            // 2) Si no existe, devolvemos 404
            if (usuario == null)
            {
                return NotFound(new { Message = "No se ha encontrado el usuario con el id especificado" });
            }

            // 3) Devolvemos el objeto proyectado
            return Ok(usuario);
        }



        [HttpPost]
        public async Task<IActionResult> crearUsuario(CreateUsuarioDto dto)
        {
            // Verificar si usuario ya existe
            var usuarioExistente = await _userManager.FindByNameAsync(dto.UserName);
            if (usuarioExistente != null)
                return BadRequest(new { message = "El usuario ya existe" });

            // Validar que el rol exista en la base
            var rol = await _context.Set<ApplicationRole>().FindAsync(dto.RoleId);
            if (rol == null)
                return BadRequest(new { message = "El rol especificado no existe" });

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                RoleId = dto.RoleId,
                name=dto.name,
                lastname=dto.lastname,
                Empresa = dto.Empresa,
                CreatedAt = DateTime.Now
            };

            var resultado = await _userManager.CreateAsync(user, dto.Password);
            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            // Crear la respuesta con los datos del usuario y el nombre del rol
            var respuesta = new
            {
                id = user.Id,
                userName = user.UserName,
                email = user.Email,
                name = user.name,
                lastname = user.lastname,
                empresa=user.Empresa,
                roles = rol.Name  // Nombre del rol asociado
            };

            return Ok(respuesta);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, UpdateUsuarioDto dto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound();

            // Actualizar campos si vienen en dto
            if (!string.IsNullOrEmpty(dto.UserName))
                user.UserName = dto.UserName;

            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.name))
                user.name = dto.name;

            if (!string.IsNullOrEmpty(dto.lastname))
                user.lastname = dto.lastname;

            if (!string.IsNullOrEmpty(dto.Empresa))
                user.Empresa = dto.Empresa;

            if (dto.RoleId.HasValue)
            {
                var role = await _context.Set<ApplicationRole>().FindAsync(dto.RoleId.Value);
                if (role == null)
                    return BadRequest(new { message = "Rol no encontrado" });

                user.RoleId = dto.RoleId.Value;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Si se quiere cambiar contraseña (opcional y separado preferiblemente)
            if (!string.IsNullOrEmpty(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                if (!passResult.Succeeded)
                    return BadRequest(passResult.Errors);
            }

            return Ok(new { message = "Usuario actualizado" });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Cambiar(int id, [FromBody] int? equipoId)
        {
            var usuario = await _userManager.FindByIdAsync(id.ToString());
            if (usuario==null)
            {
                return NotFound(new { message = "No se ha encontrado el usuario con el id especificado" });
            }

            usuario.EquipoId = equipoId;
            await _userManager.UpdateAsync(usuario);

            return Ok(usuario);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await _userManager.FindByIdAsync(id.ToString());
            if (usuario==null)
            {
                return NotFound(new { message = "No se ha encontado el usuario" });
            }

            var result = await _userManager.DeleteAsync(usuario);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new {message="Se ha eliminado correctamentre el usuario"});
        }
    }
}
