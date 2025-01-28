namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty().WithMessage("Id is required");
    }
}

public class DeleteProductHandler(CatalogDbContext catalogDb) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await catalogDb.Products.FindAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.ProductId);
        }

        catalogDb.Products.Remove(product);
        await catalogDb.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}
