﻿using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.DeliveryMethodDto
{
    public class DeliveryMethodAddRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Short Name can't be longer than 50 characters.")]
        public string ShortName { get; set; }
        [Required]
        public string DeliveryTime { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
