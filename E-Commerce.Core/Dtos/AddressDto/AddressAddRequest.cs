﻿namespace E_Commerce.Core.Dtos.AddressDto
{
    public class AddressAddRequest
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public Guid UserID { get; set; }
    }
}