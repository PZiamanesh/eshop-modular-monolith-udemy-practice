using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Data.Configurations;

public class ShopingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId).IsRequired();

        builder.Property(x => x.Quantity).IsRequired();

        builder.Property(x => x.Color);

        builder.Property(x => x.Price).IsRequired();

        builder.Property(x => x.ProductName).IsRequired();
    }
}
