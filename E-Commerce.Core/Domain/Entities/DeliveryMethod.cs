namespace E_Commerce.Core.Domain.Entities
{
    public class DeliveryMethod
    {
        public Guid DeliveryMethodID { get; set; } = Guid.NewGuid();
        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}
