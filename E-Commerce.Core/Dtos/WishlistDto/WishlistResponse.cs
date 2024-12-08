namespace E_Commerce.Core.Dtos.WishlistDto
{
    public class WishlistResponse : WishlistBase
    {
        public Guid WishlistID { get; set; }
        public DateTime AddedAt { get; set; }
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImageURL { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
