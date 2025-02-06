namespace Basket.Basket.Models;

public class ShoppingCart : Aggregate<Guid>
{
    private readonly List<ShoppingCartItem> _items = new();

    public string UserName { get; private set; }
    public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);

    public static ShoppingCart Create(
        Guid id, 
        string userName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);

        var shoppingCart = new ShoppingCart
        {
            Id = id,
            UserName = userName.ToLowerInvariant(),
        };

        return shoppingCart;
    }

    public void AddItem(
        Guid productId,
        int quantity,
        string color,
        decimal price,
        string productName
        )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var existingItem = Items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem is { })
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var newItem = new ShoppingCartItem(
                shoppingCartId: Id,
                productId,
                quantity,
                color,
                price,
                productName);

            _items.Add(newItem);
        }
    }

    internal void AddItemForDeserialization(ShoppingCartItem item)
    {
        _items.Add(item);
    }

    public void RemoveItem(Guid productId)
    {
        var existingItem = Items.FirstOrDefault(i => i.ProductId == productId); ;

        if (existingItem is { })
        {
            _items.Remove(existingItem);
        }
    }
}
