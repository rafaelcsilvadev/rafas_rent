using Amazon.CognitoIdentityProvider;
using Auth.API.Models;
using Auth.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Dependency Injection
builder.Services.AddSingleton<AmazonCognitoIdentityProviderClient>();
builder.Services.AddScoped<ICognitoRepository<ClientModel>, CognitoRepository<ClientModel>>();
builder.Services.AddScoped<ICognitoRepository<AdminModel>, CognitoRepository<AdminModel>>();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
