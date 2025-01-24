namespace Catalog.Products.Models;

public class Product : Aggregate<Guid>
{
    public string Name { get; private set; } = default!;
    public List<string> Category { get; private set; } = new();
    public string Description { get; private set; } = default!;
    public string ImageFile { get; private set; } = default!;
    public decimal Price { get; set; }

    public static Product Create(
        Guid id,
        string name,
        List<string> category,
        string description,
        string imageFile,
        decimal price
        )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var product = new Product
        {
            Id = id,
            Name = name,
            Description = description,
            Category = category,
            ImageFile = imageFile,
            Price = price
        };

        product.AddDomainEvent(new ProductCreatedEvent(product));

        return product;
    }

    public void Update(
       string name,
       List<string> category,
       string description,
       string imageFile,
       decimal price
       )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        Name = name;
        Description = description;
        Category = category;
        ImageFile = imageFile;
        
        if (Price != price)
        {
            Price = price;
            AddDomainEvent(new ProductPriceChangedEvent(this));
        }
    }
}
