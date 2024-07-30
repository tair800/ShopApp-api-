namespace ShopApp_API_.Apps.AdminApp.Dtos.ProductDto
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public List<ProductListItemDto> Items { get; set; }
    }
}
