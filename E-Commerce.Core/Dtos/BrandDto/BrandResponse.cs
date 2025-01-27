namespace E_Commerce.Core.Dtos.BrandDto
{
    public class BrandResponse : BrandBase
    {
        public Guid BrandID { get; set; }
        public long ProductLength { get; set; }
    }
}
