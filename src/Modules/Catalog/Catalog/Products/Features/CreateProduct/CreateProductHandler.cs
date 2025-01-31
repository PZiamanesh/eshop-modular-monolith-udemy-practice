namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto ProductDto) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(r => r.ProductDto.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(r => r.ProductDto.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(r => r.ProductDto.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(r => r.ProductDto.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public class CreateProductHandler(
    CatalogDbContext dbContext,
    ILogger<CreateProductHandler> logger
    ) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = CreateNewProduct(command.ProductDto);

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        return new CreateProductResult(product.Id);
    }

    private Product CreateNewProduct(ProductDto productDto)
    {
        return Product.Create(
            Guid.NewGuid(),
            productDto.Name,
            productDto.Category,
            productDto.Description,
            productDto.ImageFile,
            productDto.Price
            );
    }
}
