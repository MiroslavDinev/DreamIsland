namespace DreamIsland.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    using DreamIsland.Data.Models.Vehicles;
    using DreamIsland.Data.Models;

    public class DreamIslandDbContext : IdentityDbContext
    {
        public DreamIslandDbContext(DbContextOptions<DreamIslandDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Island> Islands { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Car>()
                .HasOne(c => c.Island)
                .WithMany(i => i.Cars)
                .HasForeignKey(c => c.IslandId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
