using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasKey(v => v.VoteID);

            builder.Property(v => v.VoteID)
                .ValueGeneratedNever();

            builder.HasOne(v => v.Review)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.ReviewID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Votes");
        }
    }
}
