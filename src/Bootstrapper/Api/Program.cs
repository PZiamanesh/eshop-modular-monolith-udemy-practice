
var builder = WebApplication.CreateBuilder(args);

// service container
builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

var app = builder.Build();

// request pipeline
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();
