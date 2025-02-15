namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketReponse(Guid Id);

public class RemoveItemFromBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}/items/{productId}",
            async ([FromRoute] string userName, 
                   [FromRoute] Guid productId,
                   ISender sender) =>
            {
                var result = await sender.Send(new RemoveItemFromBasketCommand(userName, productId));

                var response = result.Adapt<RemoveItemFromBasketReponse>();

                return Results.Ok(response);
            })
            .WithName("RemoveItemFromBasket")
            .RequireAuthorization();
    }
}
