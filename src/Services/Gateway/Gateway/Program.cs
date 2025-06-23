using Gateway.DelegateHandlers;
using Gateway.Endpoints;
using Gateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Shared.Extensions;
using Winton.Extensions.Configuration.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile($"ocelot.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"ocelot.{environment}.json", optional: false, reloadOnChange: true);

var consulServer = builder.Configuration
	.GetSection("ConsulOptions")
	.GetValue<string>("Server");

builder.Configuration.AddConsul($"Gateway/{environment}", options => options.AddConsulConfiguration(consulServer));
builder.Configuration.AddConsul($"Shared/{environment}", options => options.AddConsulConfiguration(consulServer));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddOcelot(builder.Configuration).AddDelegatingHandler<RetryHandler>(true); ;
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCors();
builder.Services.ConfigureAuthentication(builder);
builder.Services.ConfigureHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors("CorsPolicy");

app.UseSwaggerForOcelotUI(options =>
	options.PathToSwaggerGenerator = "/swagger/docs");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthCheckEndpoint();
});

await app.UseOcelot();

app.MapControllers();

app.Run();
