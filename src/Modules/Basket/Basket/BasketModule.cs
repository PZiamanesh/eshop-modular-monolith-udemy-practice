using Basket.Data;
using Basket.Data.Repository;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Data;
using Shared.Data.Interceptors;

namespace Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        // api

        // application

        services.AddScoped<IBasketRepository, BasketRepository>();
        services.Decorate<IBasketRepository, CachedBasketRepository>();

        // infra

        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<BasketDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        // api

        // application

        // infra

        app.UseMigration<BasketDbContext>();

        return app;
    }
}
