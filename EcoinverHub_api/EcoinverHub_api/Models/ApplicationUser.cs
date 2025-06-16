using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EcoinverHub_api.Models
{
    public class ApplicationUser:IdentityUser<int>
    {
    public DateTime CreatedAt { get; set; }=DateTime.Now;
        // Agregar relación directa con Role
        public int RoleId { get; set; }
        public string name { get; set; }
        public string lastname {  get; set; }
        public string Empresa { get; set; }
        public ApplicationRole Role { get; set; }

        public int? EquipoId { get; set; }
        [ForeignKey(nameof(EquipoId))]
        public Equipos Equipo { get; set; }

    }
}
