using E_Commerce.Core.Dtos.AddressDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.AddressValidator
{
    public class AddressAddRequestValidator : AbstractValidator<AddressAddRequest>
    {
        public AddressAddRequestValidator()
        {
            RuleFor(x=>x.FirstName)
                .NotEmpty().WithMessage("FirstName is required.")
                .Length(1, 50).WithMessage("FirstName can be a maximum of 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required.")
                .Length(1, 50).WithMessage("LastName can be a maximum of 50 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .Length(1, 50).WithMessage("City can be a maximum of 50 characters.");
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .Length(1, 50).WithMessage("Country can be a maximum of 50 characters.");
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.")
                .Length(1, 50).WithMessage("State can be a maximum of 50 characters.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .Length(1, 100).WithMessage("Street can be a maximum of 100 characters.");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("ZipCode is required.")
                .Length(1, 10).WithMessage("ZipCode can be a maximum of 10 characters.")
                .Matches(@"^\d{5}(?:[-\s]\d{4})?$").WithMessage("ZipCode must be a valid postal code.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.");

            RuleFor(x => x.AddressLine2)
                .Length(0, 100).WithMessage("AddressLine2 can be a maximum of 100 characters.");
        }
    }
}
