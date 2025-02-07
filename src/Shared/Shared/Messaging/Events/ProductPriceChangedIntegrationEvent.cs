namespace Shared.Messaging.Events;

public record ProductPriceChangedIntegrationEvent : IntegrationEvent
{
    public Guid ProductId { get; set; }
    public decimal Price { get; set; }
}
