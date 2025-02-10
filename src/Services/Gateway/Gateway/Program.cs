using Gateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("ocelot.json", false, true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOcelot();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.ConfigureAuthentication(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwaggerForOcelotUI(options =>
	options.PathToSwaggerGenerator = "/swagger/docs");

app.UseHttpsRedirection();

await app.UseOcelot();

app.UseAuthorization();

app.MapControllers();

app.Run();
