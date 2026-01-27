using FinanceTrackerApi.DI;
using FinanceTrackerApi.Entities;
using FinanceTrackerApi.Mappers;
using FinanceTrackerApi.Repositories;
using FinanceTrackerApi.Services.LoginOperations;
using FinanceTrackerApi.Services.TokenService;
using FinanceTrackerApi.SettingClasses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
//builder.Services.AddOpenApi(); // New .NET minimal OpenAPI (no Swagger UI)

//builder.Services.AddSingleton<FinanceTrackerApi.InMemoryDB>(); // public ctor
//builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(AccountProfileMapper));

builder.Services.AddSingleton<IEncryptionUtility, EncryptionUtility>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JWTSettings"));

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
//builder.Services.AddMediatR(typeof(Program)); //MediatR Dependency Injection package, not needed anymore after MediatR v12+

builder.Services.AddAuthenticationDependencies(builder.Configuration);
builder.Services.AddJwtSettings();

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Version = "v1",
//        Title = "ToDo API",
//        Description = "An ASP.NET Core Web API for managing ToDo items",
//        TermsOfService = new Uri("https://example.com/terms"),
//        Contact = new OpenApiContact
//        {
//            Name = "Example Contact",
//            Url = new Uri("https://example.com/contact")
//        },
//        License = new OpenApiLicense
//        {
//            Name = "Example License",
//            Url = new Uri("https://example.com/license")
//        }
//    });
//}

// middlewares
var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();

// Enable middleware to serve Swagger UI
//app.UseSwaggerUI();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = string.Empty;
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.MapOpenApi(); //New .NET minimal OpenAPI (no Swagger UI)
}

//app.Use(async (context, next) =>
//{
//    Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
//    await next();
//});

app.UseHttpsRedirection();

app.UseAuthentication();
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

//app.UseExceptionHandler("/error");
app.Run();

