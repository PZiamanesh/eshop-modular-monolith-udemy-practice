namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<ProductDto> Products);

public class GetProductsHandler(CatalogDbContext catalogDb) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await catalogDb.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var productDtos = products.Adapt<List<ProductDto>>();

        return new GetProductsResult(productDtos);
    }
}
