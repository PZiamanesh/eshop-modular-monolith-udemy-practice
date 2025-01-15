namespace Catalog.Products.Events;

public record ProductCreatedEvent(Product product) : IDomainEvent;
