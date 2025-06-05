using Microsoft.Identity;
namespace EcoinverHub_api.Models.Identity
{
    public class Anuncio : BaseModel
    {
        public string nombre { get; set; }
        public string estado { get; set; }
        public string contenido { get; set; }
    }
}
