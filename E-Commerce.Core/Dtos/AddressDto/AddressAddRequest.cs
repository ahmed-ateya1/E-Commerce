using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.AddressDto
{
    public class AddressAddRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        [Phone(ErrorMessage = "PhoneNumber must be a valid phone number.")]
        public string PhoneNumber { get; set; }
        public string AddressLine2 { get; set; }
    }
}
