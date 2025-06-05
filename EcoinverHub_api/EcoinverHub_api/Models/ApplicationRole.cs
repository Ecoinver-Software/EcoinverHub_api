using Microsoft.AspNetCore.Identity;

namespace EcoinverHub_api.Models
{
    public class ApplicationRole:IdentityRole<int>
    {
        public string Description { get; set; }
        public int Level { get; set; }
       
    }
}
