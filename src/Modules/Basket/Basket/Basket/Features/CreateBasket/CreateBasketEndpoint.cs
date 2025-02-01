namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketRequest(ShoppingCartDto ShoppingCartDto);

public record CreateBasketResponse(Guid Id);

public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (CreateBasketRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateBasketCommand(request.ShoppingCartDto));

            var response = result.Adapt<CreateBasketResponse>();

            return Results.Created($"/basket/{response.Id}", response);
        })
        .WithName("CreateBasket");
    }
}
