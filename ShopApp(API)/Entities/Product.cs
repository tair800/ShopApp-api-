namespace ShopApp_API_.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public bool isDelete { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
