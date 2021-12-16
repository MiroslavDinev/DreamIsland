namespace DreamIsland.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using DreamIsland.Data.Models;

    public class CollectibleEntityTypeConfiguration : IEntityTypeConfiguration<Collectible>
    {
        public void Configure(EntityTypeBuilder<Collectible> builder)
        {
            builder
                .HasOne(c => c.Partner)
                .WithMany(p => p.Collectibles)
                .HasForeignKey(c => c.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
