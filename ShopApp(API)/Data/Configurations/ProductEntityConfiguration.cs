using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopApp_API_.Entities;

namespace ShopApp_API_.Data.Configurations
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired(true).HasMaxLength(50);
            builder.Property(p => p.SalePrice).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(p => p.CurrentPrice).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(p => p.isDelete).HasDefaultValue(false);
            builder.Property(p => p.CreatedDate).HasDefaultValueSql("getdate()");
            builder.Property(p => p.UpdatedDate).HasDefaultValueSql("getdate()");

        }
    }
}
