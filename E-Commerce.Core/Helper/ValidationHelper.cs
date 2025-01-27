using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Helper
{
    public static class ValidationHelper
    {
        public static void ValidateModel(object? model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            if (!isValid)
                throw new ValidationException("Model is not valid");

        }
    }
}
