
namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto ProductDto) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductHandler(CatalogDbContext catalogDb) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await catalogDb.Products.FindAsync(command.ProductDto.Id, cancellationToken);

        if (product is null)
        {
            throw new Exception($"product not found: {command.ProductDto.Id}");
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
