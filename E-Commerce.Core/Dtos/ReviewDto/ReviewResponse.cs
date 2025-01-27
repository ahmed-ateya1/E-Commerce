namespace E_Commerce.Core.Dtos.ReviewDto
{
    public class ReviewResponse : ReviewBase
    {
        public Guid ReviewID { get; set; }
        public long TotalVotes { get; set; }
        public long TotalUpVotes { get; set; }
        public long TotalDownVotes { get; set; }
        public DateTime ReviewDate { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public Guid? ParentReviewID { get; set; }
        public long TotalRepliesReviews { get; set; }

        public bool HasUpVoted { get; set; }
        public bool HasDownVoted { get; set; }
    }
}
