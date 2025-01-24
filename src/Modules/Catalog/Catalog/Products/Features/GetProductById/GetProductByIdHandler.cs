namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(ProductDto Product);

public class GetProductByIdHandler(CatalogDbContext catalogDb) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await catalogDb.Products.FindAsync(query.Id, cancellationToken);

        if (product is null)
        {
            throw new Exception($"product not found: {query.Id}");
        }

        var productDto = product.Adapt<ProductDto>();

        return new GetProductByIdResult(productDto);
    }
}
