namespace EcoinverHub_api.Models
{
    public class Application:BaseModel

    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Version { get; set; }
        public string Autor { get; set; }
        public string Estado { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

        // Navegación
        public ICollection<RoleApplication> RoleApplications { get; set; }

    } 
    }
