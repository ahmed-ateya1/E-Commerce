namespace E_Commerce.Core.Dtos.TechnicalSpecificationDto
{
    public class TechnicalSpecificationBase 
    {
        public string SpecificationKey { get; set; }
        public string SpecificationValue { get; set; }
        public Guid ProductID { get; set; }
    }
}
