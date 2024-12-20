using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.OrderID);
            builder.Property(x => x.OrderID)
                .ValueGeneratedNever();
            builder.Property(x => x.OrderNumber)
                .IsRequired();
            builder.Property(x => x.OrderDate)
                .IsRequired();
            builder.Property(x => x.SubTotal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(x => x.OrderStatus)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Address)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.AddressID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.DeliveryMethod)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.DeliveryMethodID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Orders");

        }
    }
}
