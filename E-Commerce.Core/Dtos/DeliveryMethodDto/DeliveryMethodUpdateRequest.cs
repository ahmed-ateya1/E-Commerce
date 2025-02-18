﻿namespace E_Commerce.Core.Dtos.DeliveryMethodDto
{
    public class DeliveryMethodUpdateRequest
    {
        public Guid DeliveryMethodID { get; set; }
        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
