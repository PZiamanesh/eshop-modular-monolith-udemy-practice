namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketRequest(ShoppingCartItemDto ShoppingCartItemDto);

public record AddItemIntoBasketResponse(Guid Id);

public class AddItemIntoBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{userName}/items",
            async ([FromRoute] string userName, 
                   [FromBody] AddItemIntoBasketRequest request, 
                   ISender sender) =>
        {
            var command = new AddItemIntoBasketCommand(userName, request.ShoppingCartItemDto);
            var result = await sender.Send(command);
            var response = result.Adapt<AddItemIntoBasketResult>();

            return Results.Created($"/basket/{response.Id}", response);
        })
        .WithName("AddItemIntoBasket");
    }
}
