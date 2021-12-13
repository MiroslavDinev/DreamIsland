namespace DreamIsland.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    using DreamIsland.Data.Models.Vehicles;

    public class DreamIslandDbContext : IdentityDbContext
    {
        public DreamIslandDbContext(DbContextOptions<DreamIslandDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
    }
}
