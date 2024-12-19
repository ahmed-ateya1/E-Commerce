using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.IdentityEntities;

public class Product
{
    public Guid ProductID { get; set; } = Guid.NewGuid();
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public double Discount { get; set; }
    public int StockQuantity { get; set; }
    public int? WarrantyPeriod { get; set; }
    public string? Color { get; set; }
    public int ModelNumber { get; set; }
    public double AvgRating { get; set; }
    public long TotalReviews { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public Guid UserID { get; set; }
    public virtual ApplicationUser User { get; set; }

    public Guid BrandID { get; set; }
    public virtual Brand Brand { get; set; }

    public Guid CategoryID { get; set; }
    public virtual Category Category { get; set; }

    public virtual ICollection<TechnicalSpecification> TechnicalSpecifications { get; set; } = new List<TechnicalSpecification>();
    public virtual ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public decimal CalculateDiscountedPrice()
    {
        return ProductPrice - (ProductPrice * (decimal)Discount / 100);
    }
}
