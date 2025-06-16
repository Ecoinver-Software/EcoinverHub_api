using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoinverHub_api.Models
{
    public class Equipos:BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }

        public int JefeEquipoId { get; set; }


        [ForeignKey(nameof(JefeEquipoId))]
        public ApplicationUser JefeEquipo { get; set; }
        public string Empresa { get; set; }

    }
}
