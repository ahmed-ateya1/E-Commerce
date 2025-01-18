using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    public class DealConfiguration : IEntityTypeConfiguration<Deal>
    {
        public void Configure(EntityTypeBuilder<Deal> builder)
        {
            builder.HasKey(d => d.DealID);

            builder.Property(x=>x.DealID)
                .ValueGeneratedNever();

            builder.Property(x => x.Discount)
                .IsRequired();

            builder.Property(x => x.StartDate)
                .IsRequired();
            builder.Property(x => x.EndDate)
                .IsRequired();

            builder.Property(x => x.ProductID)
                .IsRequired();


            builder.HasOne(x => x.Product)
                .WithMany(x => x.Deals)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Deals");
        }
    }
}
