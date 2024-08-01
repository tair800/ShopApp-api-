namespace ShopApp_API_.Apps.AdminApp.Dtos.CategoryDto
{
    public class CategoryListDto
    {
        public int Id { get; set; }
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public List<CategoryListItemDto> Items { get; set; }
    }
}
