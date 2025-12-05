using Gtlabs.Api.AmbientData;
using Gtlabs.Api.Extensions;
using Gtlabs.AppRegistration.Extensions;
using Gtlabs.AspNet.Extensions;
using Gtlabs.Consul.Extensions;
using Gtlabs.DependencyInjections.DependencyInjectons.Extensions;
using GTLabs.Identity.Authorization.Infrastructure.Contexts;
using Gtlabs.Persistence.Extensions;
using Gtlabs.Redis.Extensions;
using Gtlabs.ServiceBus.ServiceBus.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterApp("GTLabs.Identity.Authorization");
builder.Services.AddHttpContextAccessor();
builder.Services.AddBasicFeatures();
await builder.Configuration.AddConsulConfigurationAsync();
builder.Services.AddConsulRegistration(builder.Configuration);
builder.ConfigureKestrelWithNetworkHelper();
builder.Services.RegisterServiceBus(builder.Configuration);
builder.Services.AddRedisCache(builder.Configuration);

builder.Services.AddPersistence<AuthorizationDbcontext>(builder.Configuration);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.AddConsulHealthCheck();

app.UseAuthorization();

app.MapControllers();

app.Run();