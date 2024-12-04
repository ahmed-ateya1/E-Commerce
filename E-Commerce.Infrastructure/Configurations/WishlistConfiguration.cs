using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasKey(x=>x.WishlistID);

            builder.Property(x => x.WishlistID)
                .ValueGeneratedNever();

            builder.Property(x=> x.AddedAt)
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany(x=>x.Wishlists)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Wishlists)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Wishlists");
        }
    }
}
