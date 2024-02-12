using Application;
using Application.Interfaces;
using Domain.Models;
using Domain.Repositories;
using Infrastructure;
using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Serilog;
using SubscriptionManager.API.Extensions;
using SubscriptionManager.API.Identity;
using SubscriptionManager.API.Middleware;
using SubscriptionManager.API.OptionsSetup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//registering layer services
builder.Services.AddApplication()
                .AddInfrustructure(builder.Configuration);
//adding identity user
builder.Services.AddIdentity<User, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<SubscriptionManagerDbContext>();

//adding Jwt data options 
builder.Services.ConfigureOptions<JwtOptionsSetup>();

//adding Jwt data setup 
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

//adding authentication
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

//adding authorization with custom policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityData.AdminPolicyKey, policy => policy.RequireClaim("user_role", IdentityData.AdminPolicyValue));
});

// Unit Of work repository pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// JWT token builder
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

// Retry mechanism using polly we can also add polly for other api integration
builder.Services.AddScoped<IRetryService, RetryService>();

// adding logging service using serilog
builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // applying pending migrations in development
    app.ApplyMigrations(new RetryService());
}

/// adding middleware to catch exceptions we can log them and return readable response
app.UseMiddleware<ExceptionMiddleware>();

// adding logging to log all http requests
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
