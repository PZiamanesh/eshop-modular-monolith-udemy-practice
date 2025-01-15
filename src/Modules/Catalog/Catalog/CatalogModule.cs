using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        // add api endpoint services

        // add application use cases

        // add infrastructure services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        // use api endpoint services

        // use application use cases

        // use infrastructure services
        app.UseMigration<CatalogDbContext>();

        return app;
    }
}
