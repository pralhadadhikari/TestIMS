using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IMS.web.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext )
            : base(dbContext)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<ApplicationUser>()
                .Property(e => e.MiddleName)
                .HasMaxLength(100);
            builder.Entity<ApplicationUser>()
                .Property(e => e.LastName)
                .HasMaxLength(100);

            builder.Entity<ApplicationUser>()
                .Property(e => e.Address)
                .HasMaxLength(250);
            builder.Entity<ApplicationUser>()
                .Property(e => e.ProfileUrl)
                .HasMaxLength(500);
            builder.Entity<ApplicationUser>()
                .Property(e => e.IsActive)
                .HasDefaultValue(true);

            builder.Entity<ApplicationUser>()
                .Property(e => e.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<IdentityRole>()
                .ToTable("Roles")
                .Property(p => p.Id)
                .HasColumnName("RoleId");
        }   


    }
}
