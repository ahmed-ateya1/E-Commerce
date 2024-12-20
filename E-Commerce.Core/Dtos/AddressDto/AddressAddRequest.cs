using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.AddressDto
{
    public class AddressAddRequest
    {
        [Required(ErrorMessage = "FirstName is required.")]
        [StringLength(50, ErrorMessage = "FirstName can be a maximum of 50 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required.")]
        [StringLength(50, ErrorMessage = "LastName can be a maximum of 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        [StringLength(100, ErrorMessage = "Street can be a maximum of 100 characters.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City can be a maximum of 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(50, ErrorMessage = "State can be a maximum of 50 characters.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50, ErrorMessage = "Country can be a maximum of 50 characters.")]
        public string Country { get; set; }


        [Required(ErrorMessage = "ZipCode is required.")]
        [StringLength(10, ErrorMessage = "ZipCode can be a maximum of 10 characters.")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "ZipCode must be a valid postal code.")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [Phone(ErrorMessage = "PhoneNumber must be a valid phone number.")]
        public string PhoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "AddressLine2 can be a maximum of 100 characters.")]
        public string AddressLine2 { get; set; }
    }
}
