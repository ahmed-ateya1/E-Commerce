namespace E_Commerce.Core.Dtos.ProductDto
{
    public class ProductResponse : ProductBase
    {
        public Guid ProductID { get; set; }
        public double AvgRating { get; set; }
        public long TotalReviews { get; set; }
        public decimal ProductPriceAfterDiscount { get; set; }
        public DateTime AddedAt { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public bool ProductInWishlist { get; set; }
        public List<string> ProductFilesUrl { get; set; } = new List<string>();
    }
}
