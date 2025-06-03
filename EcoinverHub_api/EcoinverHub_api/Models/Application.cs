namespace EcoinverHub_api.Models
{
    public class Application:BaseModel

    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        // Navegación
        public ICollection<RoleApplication> RoleApplications { get; set; }

    } 
    }
