using FinanceTrackerApi.Entities;
using FinanceTrackerApi.Mappers;
using FinanceTrackerApi.Repositories;
using FinanceTrackerApi.Services.LoginOperations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddAutoMapper(typeof(AccountProfileMapper).Assembly);

// Add the necessary using and package for AddNewtonsoftJson
builder.Services.AddControllers()
    .AddNewtonsoftJson(); // Requires Microsoft.AspNetCore.Mvc.NewtonsoftJson NuGet package

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.Services.AddSingleton<FinanceTrackerApi.InMemoryDB>(); // public ctor
//builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(AccountProfileMapper));

builder.Services.AddSingleton<IEncryptionUtility, EncryptionUtility>();
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
//builder.Services.AddMediatR(typeof(Program)); //MediatR Dependency Injection package, not needed anymore after MediatR v12+

builder.Services.AddSwaggerGen();

// middlewares
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.Use(async (context, next) =>
//{
//    Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
//    await next();
//});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/debug/endpoints", (IEnumerable<EndpointDataSource> endpoints) =>
{
    var data = endpoints
        .SelectMany(s => s.Endpoints)
        .Select(e => e.DisplayName)
        .ToList();

    return Results.Ok(data);
});

app.UseExceptionHandler("/error");
app.Run();