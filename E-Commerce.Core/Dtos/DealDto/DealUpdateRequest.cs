using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.DealDto
{
    public class  DealUpdateRequest
    {
        public Guid DealID { get; set; }
        public double Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ProductID { get; set; }
    }
}
