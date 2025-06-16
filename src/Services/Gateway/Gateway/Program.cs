using Gateway.DelegateHandlers;
using Gateway.Endpoints;
using Gateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile($"ocelot.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"ocelot.{environment}.json", optional: false, reloadOnChange: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
