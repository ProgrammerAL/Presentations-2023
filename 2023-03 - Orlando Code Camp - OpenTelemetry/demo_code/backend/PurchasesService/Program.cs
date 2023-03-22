#pragma warning disable IDE0058 // Expression value is never used

using Honeycomb.OpenTelemetry;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OpenTelemetry.Trace;

using ProgrammerAl.Presentations.OTel.PurchasesService;
using ProgrammerAl.Presentations.OTel.PurchasesService.EF;
using ProgrammerAl.Presentations.OTel.PurchasesService.EF.Repositories;
using ProgrammerAl.Presentations.OTel.Shared;


var builder = WebApplication.CreateBuilder(args);

//TODO: Load these values from config
var sqlDbConnectionString = "";
var honeycombApiKey = "";

if (string.IsNullOrEmpty(sqlDbConnectionString)
    || string.IsNullOrEmpty(honeycombApiKey))
{
    throw new Exception("Startup config not set");
}


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPurchasesRepository, PurchasesRepository>();
builder.Services.AddSingleton<IProductsRepository, ProductsRepository>();

builder.Services.AddPooledDbContextFactory<PurchasesServiceDbContext>((serviceProvider, optionsBuilder) =>
{
    optionsBuilder
    .UseSqlServer(sqlDbConnectionString)
    .EnableServiceProviderCaching(cacheServiceProvider: true)
        .LogTo(DatabaseOpenTelemetryHelpers.TraceSqlServerExecutedQueryInfo,
            events: PurchasesServiceDbContext.LoggingEventIds,
            minimumLevel: Microsoft.Extensions.Logging.LogLevel.Information)
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddOpenTelemetry().WithTracing(otelBuilder =>
    otelBuilder
        .AddHoneycomb(new HoneycombOptions
        {
            ServiceName = "purchases-service",
            ServiceVersion = "1.0.1-local",
            ApiKey = honeycombApiKey
        })
        .AddCommonInstrumentations()
        .AddSource(ActivitySources.PurchasesServiceSource.Name)
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#pragma warning restore IDE0058 // Expression value is never used
