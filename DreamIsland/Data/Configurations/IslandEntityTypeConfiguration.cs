namespace DreamIsland.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using DreamIsland.Data.Models;

    public class IslandEntityTypeConfiguration : IEntityTypeConfiguration<Island>
    {
        public void Configure(EntityTypeBuilder<Island> builder)
        {
            builder
                .HasOne(i => i.Partner)
                .WithMany(p => p.Islands)
                .HasForeignKey(i => i.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
