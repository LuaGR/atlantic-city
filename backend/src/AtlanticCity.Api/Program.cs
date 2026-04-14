using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using FluentValidation.AspNetCore;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Serilog;
using AtlanticCity.Application.Interfaces;
using AtlanticCity.Application.Services;
using AtlanticCity.Application.Validators;
using AtlanticCity.Domain.Interfaces;
using AtlanticCity.Infrastructure;
using AtlanticCity.Infrastructure.Persistence;
using AtlanticCity.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AtlanticCityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePedidoValidator>();

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "AtlanticCitySecretKey2026!#SECURE";
var jwtIssuer = "AtlanticCity";
var jwtAudience = "AtlanticCity";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("global", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 10;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var retryPipeline = new ResiliencePipelineBuilder()
    .AddRetry(new RetryStrategyOptions
    {
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromSeconds(1),
        BackoffType = DelayBackoffType.Exponential,
        ShouldHandle = new PredicateBuilder().Handle<Exception>()
    })
    .AddCircuitBreaker(new CircuitBreakerStrategyOptions
    {
        FailureRatio = 0.5,
        SamplingDuration = TimeSpan.FromSeconds(30),
        MinimumThroughput = 5,
        BreakDuration = TimeSpan.FromSeconds(15),
        ShouldHandle = new PredicateBuilder().Handle<Exception>()
    })
    .Build();

builder.Services.AddSingleton(retryPipeline);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseSerilogRequestLogging();

app.UseRateLimiter();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireRateLimiting("global");

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AtlanticCityDbContext>();
    db.Database.Migrate();

    if (!db.Usuarios.Any())
    {
        var admin = new AtlanticCity.Domain.Entities.Usuario
        {
            Email = "admin@atlanticcity.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            Rol = AtlanticCity.Domain.Entities.RolUsuario.Admin,
            Activo = true,
            CreatedAt = DateTime.UtcNow
        };

        var user = new AtlanticCity.Domain.Entities.Usuario
        {
            Email = "user@atlanticcity.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            Rol = AtlanticCity.Domain.Entities.RolUsuario.User,
            Activo = true,
            CreatedAt = DateTime.UtcNow
        };

        db.Usuarios.AddRange(admin, user);
        db.SaveChanges();

        Log.Information("Seed users created");
    }
}

Log.Information("Atlantic City API started");

app.Run();
