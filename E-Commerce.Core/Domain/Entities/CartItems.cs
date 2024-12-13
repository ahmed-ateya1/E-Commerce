﻿namespace E_Commerce.Core.Domain.Entities
{
    public class CartItems
    {
        public Guid CartItemsID { get; set; } = Guid.NewGuid();
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
        public string BrandName { get; set; }
    }
}