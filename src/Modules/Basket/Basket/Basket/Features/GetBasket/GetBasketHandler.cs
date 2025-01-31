namespace Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string UserName): IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCartDto);

public class GetBasketHandler(BasketDbContext dbContext) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await dbContext.ShoppingCarts
            .AsNoTracking()
            .Include(s => s.Items)
            .SingleOrDefaultAsync(b => b.UserName == query.UserName.ToLowerInvariant(), cancellationToken);

        if (basket is null)
        {
            throw new BasketNotFoundException(query.UserName);
        }

        var basketDto = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(basketDto);
    }
}
