using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x=>x.ReviewID);

            builder.Property(x => x.ReviewID)
                .ValueGeneratedNever();

            builder.Property(x => x.ReviewText)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.ReviewDate)
                .IsRequired();

            builder.Property(x => x.Rating)
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ParentReview)
                .WithMany(x => x.ChildReviews)
                .HasForeignKey(x => x.ParentReviewID)
                .OnDelete(DeleteBehavior.Restrict);


            builder.ToTable("Reviews");
        }
    }
}
