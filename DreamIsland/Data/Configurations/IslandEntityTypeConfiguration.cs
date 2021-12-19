namespace DreamIsland.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using DreamIsland.Data.Models.Islands;

    public class IslandEntityTypeConfiguration : IEntityTypeConfiguration<Island>
    {
        public void Configure(EntityTypeBuilder<Island> builder)
        {
            builder
                .HasOne(i => i.Partner)
                .WithMany(p => p.Islands)
                .HasForeignKey(i => i.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(i => i.PopulationSize)
                .WithMany(p => p.Islands)
                .HasForeignKey(i => i.PopulationSizeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(i => i.IslandRegion)
                .WithMany(r => r.Islands)
                .HasForeignKey(i => i.IslandRegionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
