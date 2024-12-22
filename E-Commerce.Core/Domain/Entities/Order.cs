using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Helper;

public class Order
{
    public Guid OrderID { get; set; } = Guid.NewGuid();
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal SubTotal { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public string PaymentIntentID { get; set; }
    public string ClientSecret { get; set; }
    public Guid UserID { get; set; }
    public ApplicationUser User { get; set; }

    public Guid AddressID { get; set; }
    public Address Address { get; set; }

    public Guid DeliveryMethodID { get; set; }
    public virtual DeliveryMethod DeliveryMethod { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public decimal GetPriceTotal()
    {
        return SubTotal + (DeliveryMethod?.Price ?? 0);
    }
}
