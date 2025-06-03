using Microsoft.AspNetCore.Identity;

namespace EcoinverHub_api.Models
{
    public class ApplicationUser:IdentityUser<int>
    {
    public DateTime CreatedAt { get; set; }=DateTime.Now;
        // Agregar relación directa con Role
        public int RoleId { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
