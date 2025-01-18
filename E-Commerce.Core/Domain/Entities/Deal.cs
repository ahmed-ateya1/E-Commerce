namespace E_Commerce.Core.Domain.Entities
{
    public class Deal
    {
        public Guid DealID { get; set; } = Guid.NewGuid();
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; }


        public bool IsActiveDeal()
        {
            return StartDate <= DateTime.UtcNow && EndDate >= DateTime.UtcNow;
        }
        public decimal PriceAfterDiscount()
        {
            return Product.ProductPrice - (Product.ProductPrice * (decimal)Discount / 100);
        }
    }
}
