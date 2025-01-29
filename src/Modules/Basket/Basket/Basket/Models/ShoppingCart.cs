namespace Basket.Basket.Models;

public class ShoppingCart : Aggregate<Guid>
{
    private readonly List<ShoppingCartItem> _items = new();

    public string UserName { get; private set; }
    public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
}
