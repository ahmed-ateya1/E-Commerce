using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    public class TechnicalSpecificationConfiguration : IEntityTypeConfiguration<TechnicalSpecification>
    {
        public void Configure(EntityTypeBuilder<TechnicalSpecification> builder)
        {
            builder.HasKey(x => x.TechnicalSpecificationID);
            builder.Property(x => x.TechnicalSpecificationID).ValueGeneratedNever();

            builder.Property(x => x.SpecificationKey)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.SpecificationValue)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.TechnicalSpecifications)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("TechnicalSpecifications");
        }
    }
}
