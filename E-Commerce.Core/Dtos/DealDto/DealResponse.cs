namespace E_Commerce.Core.Dtos.DealDto
{
    public class DealResponse
    {
        public Guid DealID { get; set; }
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductPriceAfterDiscount { get; set; }
        public string ProductImageUrl { get; set; }
        public double AvgRating { get; set; }
        public long TotalReviews { get; set; }
        public long TotalOrders { get; set; }
        public bool IsInStock { get; set; }
        public bool IsActive { get; set; }
    }
}
