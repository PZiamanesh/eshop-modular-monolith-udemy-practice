using MassTransit;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHandlers;

public class ProductPriceChangedEventHandler(
    ILogger<ProductPriceChangedEventHandler> logger,
    IBus bus
    ) : INotificationHandler<ProductPriceChangedEvent>
{
    public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Domain event handled: {notification.GetType().Name}");

        var integrationEvent = new ProductPriceChangedIntegrationEvent
        {
            ProductId = notification.Product.Id,
            Price = notification.Product.Price
        };

        await bus.Publish(integrationEvent, cancellationToken);
    }
}
