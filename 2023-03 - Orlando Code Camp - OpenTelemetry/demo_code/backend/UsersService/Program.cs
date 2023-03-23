#pragma warning disable IDE0058 // Expression value is never used

using Honeycomb.OpenTelemetry;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OpenTelemetry.Trace;

using ProgrammerAl.Presentations.OTel.Shared;
using ProgrammerAl.Presentations.OTel.UsersService.EF;
using ProgrammerAl.Presentations.OTel.UsersService.EF.Repositories;

var builder = WebApplication.CreateBuilder(args);

//TODO: Load these values from config
var databaseName = "otel-demo";
var cosmosDbConnectionString = "";
var honeycombApiKey = "";

if (string.IsNullOrEmpty(cosmosDbConnectionString)
    || string.IsNullOrEmpty(databaseName)
    || string.IsNullOrEmpty(honeycombApiKey)
    )
{
    throw new Exception("Startup config not set");
}


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUsersRepository, UsersRepository>();

builder.Services.AddPooledDbContextFactory<UsersServiceCosmosContext>((serviceProvider, optionsBuilder) =>
{

    optionsBuilder.UseCosmos(cosmosDbConnectionString, databaseName, options =>
    {
        //Limit to the given endpoint to reduce calls made on startup for multiple locations
        //  Also, we use a Serverless Cosmos DB, so there's only 1 location
        options.LimitToEndpoint(true);

        options.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Direct);
    })
        .EnableServiceProviderCaching(cacheServiceProvider: true)
        .LogTo(DatabaseOpenTelemetryHelpers.TraceCosmosDbExecutedQueryInfo,
            events: UsersServiceCosmosContext.LoggingEventIds,
            minimumLevel: Microsoft.Extensions.Logging.LogLevel.Information)
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddOpenTelemetry().WithTracing(otelBuilder =>
    otelBuilder
        .AddHoneycomb(new HoneycombOptions
        {
            ServiceName = "users-service",
            ServiceVersion = "1.0.1-local",
            ApiKey = honeycombApiKey
        })
        .AddCommonInstrumentations()
);
builder.Services.AddSingleton(TracerProvider.Default.GetTracer("users-service"));

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
