using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extensions;

public static class MassTransitExtentions
{
    public static IServiceCollection AddMassTransitWithAssemblies(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.SetInMemorySagaRepositoryProvider();

            config.AddConsumers(assemblies);

            config.AddSagas(assemblies);

            config.AddActivities(assemblies);

            
            // this is used for in-meemory bus (useful for testing)
            //config.UsingInMemory((busContext, config) =>
            //{
            //    config.ConfigureEndpoints(busContext);
            //});

            config.UsingRabbitMq((busContext, config) =>
            {
                config.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });

                config.ConfigureEndpoints(busContext);
            });
        });

        return services;
    }
}
