namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketHandler(BasketDbContext dbContext) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await dbContext.ShoppingCarts
            .SingleOrDefaultAsync(s => s.UserName == command.UserName.ToLowerInvariant(), cancellationToken);

        if (basket is null)
        {
            throw new BasketNotFoundException(command.UserName);
        }

        dbContext.ShoppingCarts.Remove(basket);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteBasketResult(true);
    }
}
