namespace E_Commerce.Core.Dtos
{
    public class RangeDto
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }

        public PaginationDto? Pagination { get; set; } = new PaginationDto();
    }
}
