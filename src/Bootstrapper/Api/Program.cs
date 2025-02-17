using Keycloak.AuthServices.Authentication;

var builder = WebApplication.CreateBuilder(args);

// host services

builder.Host.UseSerilog((hostContext, config) =>
{
    config.ReadFrom.Configuration(hostContext.Configuration);
});

// common services

Assembly[] assemblies =
[
    typeof(CatalogModule).Assembly,
    typeof(BasketModule).Assembly,
    typeof(OrderingModule).Assembly
];

builder.Services.AddCarterWithAssemblies(assemblies);

builder.Services.AddMediatRWithAssemblies(assemblies);

builder.Services.AddValidatorsFromAssemblies(assemblies);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

builder.Services.AddStackExchangeRedisCache(config =>
{
    config.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddMassTransitWithAssemblies(builder.Configuration ,assemblies);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

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

app.UseAuthentication();

app.UseAuthorization();

app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();
