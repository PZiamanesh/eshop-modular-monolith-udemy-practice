var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, config) =>
{
    config.ReadFrom.Configuration(hostContext.Configuration);
});

// service container

builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

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
