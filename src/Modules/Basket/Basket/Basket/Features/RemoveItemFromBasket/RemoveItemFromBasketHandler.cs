namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(
    string UserName,
    Guid ProductId
    ) : ICommand<RemoveItemFromBasketResult>;

public record RemoveItemFromBasketResult(Guid Id);

public class RemoveItemFromBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("UserName is required");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required");
    }
}

public class RemoveItemFromBasketHandler(BasketDbContext dbContext) : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await dbContext.ShoppingCarts
            .Include(s => s.Items)
            .SingleOrDefaultAsync(s => s.UserName == command.UserName.ToLowerInvariant(), cancellationToken);

        if (basket == null)
        {
            throw new BasketNotFoundException(command.UserName);
        }

        basket.RemoveItem(command.ProductId);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RemoveItemFromBasketResult(basket.Id);
    }
}
