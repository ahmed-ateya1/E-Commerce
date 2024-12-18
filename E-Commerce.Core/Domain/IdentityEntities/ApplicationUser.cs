using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.AuthenticationDto;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? OTPCode { get; set; }
        public DateTime? OTPExpiration { get; set; }
        public string FullName { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
        public virtual ICollection<Notifications> Notifications { get; set; } = [];
        public virtual ICollection<Product> Products { get; set; } = [];
        public virtual ICollection<Wishlist> Wishlists { get; set; } = [];
        public virtual ICollection<Review> Reviews { get; set; } = [];
        public virtual ICollection<Vote> Votes { get; set; } = [];
        public virtual ICollection<Order> Orders { get; set; } = [];
        public virtual ICollection<Address> Addresses { get; set; } = [];
    }
}
