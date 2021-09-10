using JwtPractice.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtPractice.Infrastructure.Authentication
{
    // public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    // public class ApplicationDbContext : IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Candidate>()
                .HasOne(c => c.User)
                .WithOne(u => u.Candidate)
                .HasForeignKey<ApplicationUser>(c => c.Email);
        }
    }
}
