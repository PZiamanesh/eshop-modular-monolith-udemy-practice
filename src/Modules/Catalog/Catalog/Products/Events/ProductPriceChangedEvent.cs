namespace Catalog.Products.Events;

public record ProductPriceChangedEvent(Product product) : IDomainEvent;
