namespace Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string UserName): IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCartDto);

public class GetBasketHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketAsync(userName: query.UserName, asNoTracking: true, cancellationToken);

        if (basket is null)
        {
            throw new BasketNotFoundException(query.UserName);
        }

        var basketDto = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(basketDto);
    }
}
