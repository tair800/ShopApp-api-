using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopApp_API_.Entities;

namespace ShopApp_API_.Data.Configurations
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(p => p.isDelete).HasDefaultValue(false);
            builder.Property(p => p.CreatedDate).HasDefaultValueSql("getdate()");
            builder.Property(p => p.UpdatedDate).HasDefaultValueSql("getdate()");
        }
    }
}
