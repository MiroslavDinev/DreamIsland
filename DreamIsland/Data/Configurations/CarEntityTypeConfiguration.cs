namespace DreamIsland.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using DreamIsland.Data.Models.Vehicles;

    public class CarEntityTypeConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder
                .HasOne(c => c.Island)
                .WithMany(i => i.Cars)
                .HasForeignKey(c => c.IslandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(c => c.Partner)
                .WithMany(p => p.Cars)
                .HasForeignKey(c => c.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
