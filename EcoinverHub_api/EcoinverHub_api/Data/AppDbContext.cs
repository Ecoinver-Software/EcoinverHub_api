using Microsoft.EntityFrameworkCore;
using EcoinverHub_api.Models;

namespace EcoinverHub_api.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        
        }
        
    }
}
