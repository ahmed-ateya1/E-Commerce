using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    public class OrderItemsConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.OrderItemID);
            builder.Property(x => x.OrderItemID)
                .ValueGeneratedNever();

            builder.Property(x => x.Quantity)
                .IsRequired();
            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)").
                IsRequired();
            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Product)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("OrderItems");
        }
    }
}
