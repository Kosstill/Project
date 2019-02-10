namespace Project.Utilities
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Project.Models;

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Organization> Organizations { set; get; }
        public DbSet<Country> Countries { set; get; }
        public DbSet<Business> Businesses { set; get; }
        public DbSet<Family> Families { set; get; }
        public DbSet<Offering> Offerings { set; get; }
        public DbSet<Department> Departments { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Organization>()
                .HasMany(o => o.Countries)
                .WithOne(c => c.Organization)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Country>()
                .HasMany(o => o.Businesses)
                .WithOne(b => b.Country)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Business>()
                .HasMany(b => b.Families)
                .WithOne(f => f.Business)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Family>()
                .HasMany(f => f.Offerings)
                .WithOne(o => o.Family)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Offering>()
                .HasMany(o => o.Departments)
                .WithOne(d => d.Offering)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}