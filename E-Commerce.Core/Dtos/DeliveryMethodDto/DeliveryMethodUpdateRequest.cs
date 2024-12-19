using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Dtos.DeliveryMethodDto
{
    public class DeliveryMethodUpdateRequest
    {
        [Required]
        public Guid DeliveryMethodID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Short Name can't be longer than 50 characters.")]
        public string ShortName { get; set; }
        [Required]
        public string DeliveryTime { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Description can't be longer than 100 characters.")]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
