// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoinverHub_api.Models;
using EcoinverHub_api.Data;
using EcoinverHub_api.Services;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using EcoinverHub_api.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EcoinverHub_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JWTService _jwtService;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthController(
            AppDbContext context,
            JWTService jwtService,
            ILogger<AuthController> logger,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
            _userManager = userManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginRequest)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserName == loginRequest.Username || u.Email == loginRequest.Username);

                if (user == null)
                    return BadRequest(new { message = "Credenciales Inválidas" });

                if (user.Role.Name == null)
                    return StatusCode(500, new { message = "El usuario no tiene un rol asignado." });
                var passwordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
                if (!passwordValid)
                    return BadRequest(new { message = "Credenciales Inválidas" });

              

                var token = _jwtService.GenerateToken(user);

                return Ok(new
                {
                    token = token,
                    id = user.Id,
                    role = user.Role?.Name
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en api/auth/login");
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        //AQUI QUIERO EL METODO PARA 
        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetProfile()
        {
            // 1) Se obtiene el claim "sub" o ClaimTypes.NameIdentifier (según config de JWTService).
            //    En este ejemplo asumimos que en el token pusiste el ClaimTypes.NameIdentifier con el valor de user.Id.
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { message = "Token inválido o expirado" });
            }

            // 2) Intentamos parsear el ID a entero (típicamente tu Id es int; si fuera string, omites el parseo).
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Claim de usuario inválido" });
            }

            // 3) Buscamos en la base de datos el usuario incluyendo el rol
            var usuario = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (usuario == null)
            {
                return NotFound(new { message = "No se encontró el usuario asociado al token" });
            }

            // 4) Proyectamos únicamente los campos que queremos devolver
            var perfil = new
            {
                usuario.Id,
                usuario.UserName,
                usuario.Email,
                Roles = usuario.Role != null ? usuario.Role.Name : null,
                usuario.CreatedAt
                // (Puedes agregar más campos aquí si los necesitas en el perfil)
            };

            return Ok(perfil);
        }


        // Método privado para verificar contraseña
        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                var parts = hashedPassword.Split(':');
                if (parts.Length != 2) return false;

                var hash = parts[0];
                var salt = parts[1];

                using var sha256 = SHA256.Create();
                var saltedPassword = password + salt;
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                var computedHash = Convert.ToBase64String(hashedBytes);

                return hash == computedHash;
            }
            catch
            {
                return false;
            }
        }

        // Método privado para hashear contraseña (por si lo necesitas después)
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var salt = Guid.NewGuid().ToString();
            var saltedPassword = password + salt;
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            var hashedPassword = Convert.ToBase64String(hashedBytes);
            return $"{hashedPassword}:{salt}";
        }
    }
}