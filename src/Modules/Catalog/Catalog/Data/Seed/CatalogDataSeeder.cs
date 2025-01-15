namespace Catalog.Data.Seed;

public class CatalogDataSeeder(CatalogDbContext catalogDb) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        if (!await catalogDb.Products.AnyAsync())
        {
            await catalogDb.Products.AddRangeAsync(InitialData.Products);
            await catalogDb.SaveChangesAsync();
        }
    }
}
