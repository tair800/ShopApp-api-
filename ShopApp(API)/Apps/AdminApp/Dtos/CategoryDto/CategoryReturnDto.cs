namespace ShopApp_API_.Apps.AdminApp.Dtos.CategoryDto
{
    public class CategoryReturnDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
