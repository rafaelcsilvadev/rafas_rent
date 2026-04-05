using Amazon.CognitoIdentityProvider;
using Auth.API.Models;
using Auth.API.Repositories;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var userPollId = Environment.GetEnvironmentVariable("AWS_COGNITO_USER_POOL_ID");

// Authentication Middleware
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = $"https://cognito-idp.us-east-1.amazonaws.com/{userPollId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();
    

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
