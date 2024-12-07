using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Dtos.TechnicalSpecificationDto
{
    public class TechnicalSpecificationBase 
    {
        [Required(ErrorMessage = "Specification Key can't be balnk.")]
        [StringLength(100, ErrorMessage = "Specification Key can't be more than 100 characters.")]
        public string SpecificationKey { get; set; }
        [Required(ErrorMessage = "Specification Value can't be balnk.")]
        [StringLength(500, ErrorMessage = "Specification Value can't be more than 500 characters.")]
        public string SpecificationValue { get; set; }

        [Required(ErrorMessage = "Product Id can't be balnk.")]
        public Guid ProductID { get; set; }
    }
}
