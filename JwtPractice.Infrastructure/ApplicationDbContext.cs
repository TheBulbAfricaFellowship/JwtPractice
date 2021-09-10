using JwtPractice.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtPractice.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .Property("Id").UseIdentityColumn();

            builder.Entity<ApplicationRole>()
                .Property("Id").UseIdentityColumn();

            builder.Entity<Candidate>()
                .HasOne(c => c.User)
                .WithOne();

            builder.Entity<Administrator>()
                .HasOne(a => a.User)
                .WithOne();
        }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<Administrator> Administrators { get; set; }
    }
}
