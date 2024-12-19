using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(e => e.AddressID);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Street)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(e => e.State)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.ZipCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);
            builder.Property(e => e.CreatedAt)
                .IsRequired();
            builder.HasOne(d => d.User)
                .WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Addresses_AspNetUsers");

            builder.ToTable("Addresses");
        }
    }
}
