using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatchApp.Controllers;
using SolarWatchApp.DataServices;
using SolarWatchApp.DataServices.Authentication;
using SolarWatchApp.DataServices.Repositories;
using SolarWatchApp.JsonProcessor;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Load environment variables from the .env file
DotNetEnv.Env.Load();

// Load configuration from appsettings.json
configuration.AddJsonFile("appsettings.json");

ConfigureServices(builder, configuration);
var app = builder.Build();

Configure(app);

app.Run();

void ConfigureServices(WebApplicationBuilder builder, IConfiguration configuration)
{
    AddDatabaseServices(builder, configuration);
    AddHttpClientServices(builder);
    AddCustomServices(builder);
    AddAuthenticationServices(builder);
    AddSwaggerServices(builder);
}

void AddDatabaseServices(WebApplicationBuilder builder, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("SolarWatchContext");
    builder.Services.AddDbContext<SolarWatchContext>(options =>
    {
        options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });
    });
    builder.Services.AddDbContext<UserContext>();
    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<UserContext>()
        .AddDefaultTokenProviders();
}

void AddHttpClientServices(WebApplicationBuilder builder)
{
    builder.Services.AddHttpClient<SunsetSunriseController>();
}

void AddCustomServices(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<ICityRepository, CityRepository>();
    builder.Services.AddTransient<IJsonProcessor, JsonProcessor>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddControllers();
}

void AddAuthenticationServices(WebApplicationBuilder builder)
{
    var validIssuer = configuration["ValidIssuer"];
    var validAudience = configuration["ValidAudience"];
    var issuesSigningKey = configuration["IssuerSigningKey"];

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuesSigningKey)),
            };
        });
}

void AddSwaggerServices(WebApplicationBuilder builder)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "SolarWatch API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
}

void Configure(WebApplication app)
{
    ConfigureCors(app);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sunset Sunrise V1");
        });
    }

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}

void ConfigureCors(WebApplication app)
{
    app.UseCors(options =>
    {
        options.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
}
public partial class Program { }