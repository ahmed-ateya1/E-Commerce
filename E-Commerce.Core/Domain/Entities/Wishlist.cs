using E_Commerce.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Domain.Entities
{
    public class Wishlist
    {
        public Guid WishlistID { get; set; } = Guid.NewGuid();
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; }
        public Guid UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
