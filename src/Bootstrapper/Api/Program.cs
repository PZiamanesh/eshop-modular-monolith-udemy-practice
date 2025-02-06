using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// service container

builder.Host.UseSerilog((hostContext, config) =>
{
    config.ReadFrom.Configuration(hostContext.Configuration);
});

// common services

Assembly[] assemblies =
[
    typeof(CatalogModule).Assembly,
    typeof(BasketModule).Assembly
];

builder.Services.AddCarter(configurator: config =>
{
    foreach (var assembly in assemblies)
    {
        var carterEndpoints = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

        config.WithModules(carterEndpoints);
    }
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assemblies);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssemblies(assemblies);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

// module specific services

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

var app = builder.Build();

// request pipeline

app.MapCarter();

app.UseSerilogRequestLogging();

app.UseExceptionHandler(ops => { });

app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();
