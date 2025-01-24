namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto ProductDto) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductHandler(CatalogDbContext catalogDb) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = CreateNewProduct(command.ProductDto);

        catalogDb.Products.Add(product);
        await catalogDb.SaveChangesAsync();

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
