using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notifications>
    {
        public void Configure(EntityTypeBuilder<Notifications> builder)
        {
            builder.HasKey(x => x.NotificationID);

            builder.Property(x => x.NotificationID)
                .ValueGeneratedNever();

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.IsRead)
                .IsRequired();

            builder.Property(x => x.NotificationType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ReferenceURL)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(x=>x.User)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Notifications");
        }
    }
}
