using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class DealAddRequest
{
    public double Discount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid ProductID { get; set; }
}