namespace Basket.Data.CacheSerialization;

public class ShoppingCartJsonDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public List<ShoppingCartItemJsonDto> Items { get; set; } = new();

    public static ShoppingCartJsonDto ToJsonDto(ShoppingCart cart)
    {
        return new ShoppingCartJsonDto
        {
            Id = cart.Id,
            UserName = cart.UserName,
            Items = cart.Items.Select(ShoppingCartItemJsonDto.ToJsonDto).ToList()
        };
    }

    public ShoppingCart ToDomain()
    {
        var cart = ShoppingCart.Create(Id, UserName);

        foreach (var itemDto in Items)
        {
            var item = new ShoppingCartItem(
                itemDto.Id,
                cart.Id,
                itemDto.ProductId,
                itemDto.Quantity,
                itemDto.Color,
                itemDto.Price,
                itemDto.ProductName);

            cart.AddItemForDeserialization(item);
        }

        return cart;
    }
}


public class ShoppingCartItemJsonDto
{
    public Guid Id { get; set; }
    public Guid ShoppingCartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string Color { get; set; }
    public decimal Price { get; set; }
    public string ProductName { get; set; }

    public static ShoppingCartItemJsonDto ToJsonDto(ShoppingCartItem item)
    {
        return new ShoppingCartItemJsonDto
        {
            Id = item.Id,
            ShoppingCartId = item.ShoppingCartId,
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            Color = item.Color,
            Price = item.Price,
            ProductName = item.ProductName
        };
    }
}