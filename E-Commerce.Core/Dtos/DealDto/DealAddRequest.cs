using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class DealAddRequest
{
    [Required(ErrorMessage = "Discount can't be blank!")]
    [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100!")]
    public decimal Discount { get; set; }

    [Required(ErrorMessage = "Start date can't be blank!")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date can't be blank!")]
    [CustomValidation(typeof(DealAddRequest), "ValidateEndDate")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Product can't be blank!")]
    public Guid ProductID { get; set; }

    public static ValidationResult ValidateEndDate(DateTime endDate, ValidationContext context)
    {
        var instance = context.ObjectInstance as DealAddRequest;
        if (instance != null && endDate <= instance.StartDate)
        {
            return new ValidationResult("End date must be greater than start date.");
        }
        return ValidationResult.Success;
    }
}