namespace DreamIsland.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using DreamIsland.Data.Models.Celebrities;

    public class CelebrityEntityTypeConfiguration : IEntityTypeConfiguration<Celebrity>
    {
        public void Configure(EntityTypeBuilder<Celebrity> builder)
        {
            builder
                .HasOne(c => c.Partner)
                .WithMany(p => p.Celebrities)
                .HasForeignKey(c => c.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(c => c.CelebritiesGallery)
                .WithOne(g => g.Celebrity)
                .HasForeignKey(g => g.CelebrityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
