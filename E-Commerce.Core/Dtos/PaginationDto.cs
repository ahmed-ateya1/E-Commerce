namespace E_Commerce.Core.Dtos
{
    public class PaginationDto
    {
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }

}
