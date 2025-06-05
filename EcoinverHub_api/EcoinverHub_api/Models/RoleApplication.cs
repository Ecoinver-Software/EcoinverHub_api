using static System.Net.Mime.MediaTypeNames;

namespace EcoinverHub_api.Models
{
    public class RoleApplication:BaseModel
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ApplicationId { get; set; }
        public Application Application { get; set; }
    }
}
