namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto ProductDto) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(r => r.ProductDto.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(r => r.ProductDto.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(r => r.ProductDto.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public class UpdateProductHandler(CatalogDbContext catalogDb) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await catalogDb.Products.FindAsync(command.ProductDto.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.ProductDto.Id);
        }

        UpdateProductWithNewValues(product, command.ProductDto);

        catalogDb.Products.Update(product);
        await catalogDb.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }

    private void UpdateProductWithNewValues(Product product, ProductDto productDto)
    {
        product.Update(
            productDto.Name,
            productDto.Category,
            productDto.Description,
            productDto.ImageFile,
            productDto.Price
            );
    }
}
