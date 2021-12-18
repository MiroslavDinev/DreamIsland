namespace DreamIsland.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using DreamIsland.Data.Models;

    public class PartnerEntityTypeConfiguration : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Partner>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
