using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    public class ProductImagesConfiguration : IEntityTypeConfiguration<ProductImages>
    {
        public void Configure(EntityTypeBuilder<ProductImages> builder)
        {
            builder.HasKey(x => x.ProductImageID);

            builder.Property(x => x.ProductImageID)
                .ValueGeneratedNever();

            builder.Property(x => x.ImageURL)
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ProductImages)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("ProductImages");
        }
    }
}
