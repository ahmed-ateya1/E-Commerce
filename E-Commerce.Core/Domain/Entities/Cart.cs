namespace E_Commerce.Core.Domain.Entities
{
    public class Cart
    {
        public Guid CartID { get; set; } = Guid.NewGuid();
        public List<CartItems> CartItems { get; set; } = [];
    }
}
