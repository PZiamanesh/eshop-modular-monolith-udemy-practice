namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketCommand(
    string UserName,
    ShoppingCartItemDto ShoppingCartItemDto
    ) : ICommand<AddItemIntoBasketResult>;

public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("UserName is required");

        RuleFor(x => x.ShoppingCartItemDto.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required");

        RuleFor(x => x.ShoppingCartItemDto.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.ShoppingCartItemDto.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");

        RuleFor(x => x.ShoppingCartItemDto.ProductName)
            .NotEmpty()
            .WithMessage("ProductName is required");
    }
}

public class AddItemIntoBasketHandler(
    IBasketRepository repository, 
    ISender sender
    ) : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketAsync(userName: command.UserName, asNoTracking: false, cancellationToken);

        if (basket is null)
        {
            throw new BasketNotFoundException(command.UserName);
        }

        var result = await sender.Send(new GetProductByIdQuery(command.ShoppingCartItemDto.ProductId));

        basket.AddItem(
            productId: command.ShoppingCartItemDto.ProductId,
            quantity: command.ShoppingCartItemDto.Quantity,
            color: command.ShoppingCartItemDto.Color,
            price: result.Product.Price,
            productName: result.Product.Name
            );

        await repository.SaveChangesAsync(userName: command.UserName, cancellationToken: cancellationToken);

        return new AddItemIntoBasketResult(basket.Id);
    }
}
