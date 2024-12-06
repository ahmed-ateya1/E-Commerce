namespace E_Commerce.Core.Dtos.CategoryDto
{
    public class CategoryResponse : CategoryBase
    {
        public Guid CategoryID { get; set; }
        public string? CategoryImageURL { get; set; }
        public Guid? ParentCategoryID { get; set; }
        public string? ParentCategoryName { get; set; }
    }
}
