﻿namespace ShopApp_API_.Apps.AdminApp.Dtos.ProductDto
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public int CategoryId { get; set; }
    }
}
