namespace ShopApp_API_.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public bool isDelete { get; set; }
        public List<Product> Products { get; set; }
    }
}
