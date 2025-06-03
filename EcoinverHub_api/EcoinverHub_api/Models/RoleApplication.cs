using static System.Net.Mime.MediaTypeNames;

namespace EcoinverHub_api.Models
{
    public class RoleApplication:BaseModel
    {
        public int RoleId { get; set; }
        public ApplicationRole Role { get; set; }

        public int ApplicationId { get; set; }
        public Application Application { get; set; }
    }
}
