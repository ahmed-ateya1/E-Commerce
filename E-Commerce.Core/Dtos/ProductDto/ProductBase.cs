using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Dtos.ProductDto
{
    public class ProductBase
    {
        [Required(ErrorMessage = "Product Name can't be blank.")]
        [StringLength(50, ErrorMessage = "Product Name can't be longer than 50 characters.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Product Description can't be blank.")]
        [StringLength(500, ErrorMessage = "Product Description can't be longer than 500 characters.")]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "Product Price can't be blank.")]
        public decimal ProductPrice { get; set; }
        [Required(ErrorMessage = "Product Discount can't be blank.")]
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public double Discount { get; set; }
        [Required(ErrorMessage = "Stock Quantity can't be blank.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity must be greater than 0.")]
        public int StockQuantity { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Warranty Period must be greater than 0.")]
        public int? WarrantyPeriod { get; set; }
        [StringLength(50, ErrorMessage = "Color can't be longer than 50 characters.")]
        public string? Color { get; set; }

        [Required(ErrorMessage = "ModelNumber can't be blank.")]
        [Range(0, int.MaxValue, ErrorMessage = "ModelNumber must be greater than 0.")]
        public int ModelNumber { get; set; }
        [Required(ErrorMessage = "BrandID can't be blank.")]
        public Guid BrandID { get; set; }
        [Required(ErrorMessage = "CategoryID can't be blank.")]
        public Guid CategoryID { get; set; }
    }
}
