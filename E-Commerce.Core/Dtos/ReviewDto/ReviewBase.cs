namespace E_Commerce.Core.Dtos.ReviewDto
{
    public class ReviewBase 
    {
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public Guid ProductID { get; set; }
    }
}
