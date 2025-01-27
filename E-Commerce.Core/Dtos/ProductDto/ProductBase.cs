using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.ProductDto
{
    public class ProductBase
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public double Discount { get; set; }
        public int StockQuantity { get; set; }
        public int? WarrantyPeriod { get; set; }
        public string? Color { get; set; }
        public int ModelNumber { get; set; }
        public Guid BrandID { get; set; }
        public Guid CategoryID { get; set; }
    }
}
