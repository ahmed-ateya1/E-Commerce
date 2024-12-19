using E_Commerce.Core.Domain.IdentityEntities;

namespace E_Commerce.Core.Domain.Entities
{
    public class Address
    {
        public Guid AddressID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string AddressLine2 { get; set; } 
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid UserID { get; set; }
        public ApplicationUser User { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}
