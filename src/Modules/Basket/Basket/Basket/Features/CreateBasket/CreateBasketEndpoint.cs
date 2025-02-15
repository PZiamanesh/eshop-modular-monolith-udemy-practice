using System.Security.Claims;

namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketRequest(ShoppingCartDto ShoppingCartDto);

public record CreateBasketResponse(Guid Id);

public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (CreateBasketRequest request, ISender sender, ClaimsPrincipal user) =>
        {
            var userName = user.Identity!.Name;
            var updatedBasket = request.ShoppingCartDto with { UserName = userName };

            var result = await sender.Send(new CreateBasketCommand(updatedBasket));

            var response = result.Adapt<CreateBasketResponse>();

            return Results.Created($"/basket/{response.Id}", response);
        })
        .WithName("CreateBasket")
        .RequireAuthorization();
    }
}
