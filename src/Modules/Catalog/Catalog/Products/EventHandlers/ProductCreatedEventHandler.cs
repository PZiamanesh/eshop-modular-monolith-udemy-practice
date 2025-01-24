namespace Catalog.Products.EventHandlers;

public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger) : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"domain event handled: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
