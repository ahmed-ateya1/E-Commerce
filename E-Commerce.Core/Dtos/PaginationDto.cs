public class PaginationDto
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } 
    public string? SortDirection { get; set; } = "asc"; 
}
