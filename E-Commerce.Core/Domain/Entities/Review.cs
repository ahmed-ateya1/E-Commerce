using E_Commerce.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Domain.Entities
{
    public class Review
    {
        public Guid ReviewID { get; set; } = Guid.NewGuid();
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public long TotalVotes { get; set; }
        public DateTime ReviewDate { get; set; }

        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; }

        public Guid UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Guid? ParentReviewID { get; set; }
        public virtual Review ParentReview { get; set; }

        public virtual ICollection<Review> ChildReviews { get; set; } = [];
        public virtual ICollection<Vote> Votes { get; set; } = [];
    }
}
