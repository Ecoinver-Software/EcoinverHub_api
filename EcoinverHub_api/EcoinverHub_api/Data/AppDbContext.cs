using Microsoft.EntityFrameworkCore;
using EcoinverHub_api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EcoinverHub_api.Models.Identity;

namespace EcoinverHub_api.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<RoleApplication> RoleApplications { get; set; }
        public DbSet<Anuncio> Anuncios { get; set; }
        public DbSet<Equipos> Equipos { get; set; } 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b => b.ToTable("users"));
            builder.Entity<ApplicationRole>(b => b.ToTable("roles"));
            builder.Entity<RoleApplication>(b => b.ToTable("roleapplications"));
            builder.Entity<Application>(b => b.ToTable("applications"));
            builder.Entity<Anuncio>(b => b.ToTable("anuncios"));

            builder.Entity<IdentityUserRole<int>>(b => b.ToTable("userroles"));
            
        }
        public DbSet<EcoinverHub_api.Models.Identity.Anuncio> Anuncio { get; set; } = default!;





    }
}
