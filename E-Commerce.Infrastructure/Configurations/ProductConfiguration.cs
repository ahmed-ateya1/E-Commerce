using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x=>x.ProductID);

            builder.Property(x => x.ProductID)
                .ValueGeneratedNever();

            builder.Property(x => x.ProductName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ProductDescription)
                .HasMaxLength(800)
                .IsRequired();

            builder.Property(x => x.ProductPrice)
                .IsRequired();

            builder.Property(x => x.StockQuantity)
                .IsRequired();

            builder.Property(x=>x.WarrantyPeriod)
                .IsRequired(false);

            builder.Property(x => x.Color)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ModelNumber)
                .IsRequired();

            builder.Property(x=>x.Discount)
                .IsRequired();

            builder.HasOne(x=>x.User)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Brand)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Products");
        }
    }
}
