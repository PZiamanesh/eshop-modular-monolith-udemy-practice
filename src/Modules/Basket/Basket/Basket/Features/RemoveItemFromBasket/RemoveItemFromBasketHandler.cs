using Shared.Exceptions;

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

public class RemoveItemFromBasketHandler(IBasketRepository repository) : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketAsync(userName: command.UserName, asNoTracking: false, cancellationToken);

        if (basket == null)
        {
            throw new BasketNotFoundException(command.UserName);
        }

        var product = basket.Items.FirstOrDefault(p => p.ProductId ==  command.ProductId);

        if (product == null)
        {
            throw new NotFoundException("Product", command.ProductId);
        }

        basket.RemoveItem(command.ProductId);
        await repository.SaveChangesAsync(cancellationToken);

        return new RemoveItemFromBasketResult(basket.Id);
    }
}
