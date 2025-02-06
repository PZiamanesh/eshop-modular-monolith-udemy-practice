namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCartDto): ICommand<CreateBasketResult>;

public record CreateBasketResult(Guid Id);

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCartDto.UserName)
            .NotEmpty()
            .WithMessage("UserName is required");
    }
}

public class CreateBasketHandler(IBasketRepository repository) : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = CreateBasket(command.ShoppingCartDto);
        
        await repository.CreateBasketAsync(basket, cancellationToken);

        return new CreateBasketResult(basket.Id);
    }

    private ShoppingCart CreateBasket(ShoppingCartDto shoppingCartDto)
    {
        var shoppingCart = ShoppingCart.Create(
            id: Guid.NewGuid(),
            userName: shoppingCartDto.UserName
            );

        foreach (var item in shoppingCartDto.Items)
        {
            shoppingCart.AddItem(
                productId: item.ProductId,
                quantity: item.Quantity,
                color: item.Color,
                price: item.Price,
                productName: item.ProductName
                );
        }

        return shoppingCart;
    }
}
