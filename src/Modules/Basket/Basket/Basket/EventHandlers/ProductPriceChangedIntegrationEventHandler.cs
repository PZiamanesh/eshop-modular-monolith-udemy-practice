using Basket.Basket.Features.UpdateItemPriceInBasket;

namespace Basket.Basket.EventHandlers;

public class ProductPriceChangedIntegrationEventHandler(
    ILogger<ProductPriceChangedIntegrationEventHandler> logger,
    ISender sender
    ) : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        var message = context.Message;

        logger.LogInformation("Integration event handled: {IntegrationEvent}", message.GetType().Name);

        var result = await sender.Send(new UpdateItemPriceInBasketCommand(message.ProductId, message.Price));

        if (!result.IsSuccess)
        {
            logger.LogError("Error updating price in basket for productId: {ProductId}", message.ProductId);
        }

        logger.LogError("Price for productId: {ProductId} updated in basket" , message.ProductId);
    }
}
