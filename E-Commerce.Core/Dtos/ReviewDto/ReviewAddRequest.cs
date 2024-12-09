namespace E_Commerce.Core.Dtos.ReviewDto
{
    public class ReviewAddRequest : ReviewBase
    {
        public Guid? ParentReviewID { get; set; }
    }
}
