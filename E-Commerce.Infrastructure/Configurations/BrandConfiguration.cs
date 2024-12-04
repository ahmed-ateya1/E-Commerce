using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(x => x.BrandID);

            builder.Property(x=>x.BrandID)
                .ValueGeneratedNever();

            builder.Property(x => x.BrandName)
                .HasMaxLength(100)
                .IsRequired();

           builder.ToTable("Brands");
        }
    }
}
