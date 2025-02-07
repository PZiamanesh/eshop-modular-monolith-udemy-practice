using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extensions;

public static class MassTransitExtentions
{
    public static IServiceCollection AddMassTransitWithAssemblies(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.SetInMemorySagaRepositoryProvider();

            config.AddConsumers(assemblies);

            config.AddSagas(assemblies);

            config.AddActivities(assemblies);

            config.UsingInMemory((busContext, config) =>
            {
                config.ConfigureEndpoints(busContext);
            });
        });

        return services;
    }
}
