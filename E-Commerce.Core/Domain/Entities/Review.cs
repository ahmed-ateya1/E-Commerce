using E_Commerce.Core.Domain.IdentityEntities;

namespace E_Commerce.Core.Domain.Entities
{
    public class Review
    {
        public Guid ReviewID { get; set; } = Guid.NewGuid();
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public long TotalVotes { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

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
