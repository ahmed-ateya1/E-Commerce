using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.CategoryID);
            builder.Property(x => x.CategoryID).ValueGeneratedNever();

            builder.Property(x => x.CategoryName).HasMaxLength(50).IsRequired();

            builder.Property(x => x.CategoryImageURL).IsRequired(false);

            builder.HasOne(x => x.ParentCategory)
                .WithMany(x => x.SubCategories)
                .HasForeignKey(x => x.ParentCategoryID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Categories");

        }
    }
}
