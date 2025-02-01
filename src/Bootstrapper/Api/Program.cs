using FluentValidation;
using Shared.Behaviors;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, config) =>
{
    config.ReadFrom.Configuration(hostContext.Configuration);
});

// service container

builder.Services.AddCarterWithAssemblies(
    typeof(CatalogModule).Assembly,
    typeof(BasketModule).Assembly
    );

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(
        typeof(CatalogModule).Assembly,
        typeof(BasketModule).Assembly
        );

    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssemblies([
    typeof(CatalogModule).Assembly,
    typeof(BasketModule).Assembly
]);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

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
