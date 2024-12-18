using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Helper;

namespace E_Commerce.Core.Domain.Entities
{
    public class Order
    {
        public Guid OrderID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; } 
        public OrderStatus OrderStatus { get; set; }  = OrderStatus.Pending;

        public Guid UserID { get; set; }
        public ApplicationUser User { get; set; }

        public Guid AddressID { get; set; }
        public Address Address { get; set; }
        public Guid DeliveryMethodID { get; set; } 
        public DeliveryMethod DeliveryMethod { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
