namespace E_Commerce.Core.Domain.Entities
{
    public class Category
    {
        public Guid CategoryID { get; set; } = Guid.NewGuid();
        public string CategoryName { get; set; }
        public string? CategoryImageURL { get; set; }

        public virtual ICollection<Product> Products { get; set; } = [];

        public Guid? ParentCategoryID { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; } = [];
    }
}
