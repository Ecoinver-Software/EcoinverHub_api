using Microsoft.Identity;
namespace EcoinverHub_api.Models.Identity
{
    public class Anuncio : BaseModel
    {
        public string Creador { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string Contenido { get; set; }
    }
}
