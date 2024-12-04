using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class ProductResponse : ProductBase
    {
        public Guid ProductID { get; set; }
        public double AvgRating { get; set; }
        public long TotalReviews { get; set; }
        public DateTime AddedAt { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
    }
}
