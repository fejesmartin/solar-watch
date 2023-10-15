using Microsoft.EntityFrameworkCore;
using SolarWatchApp.Controllers;
using SolarWatchApp.DataServices;
using SolarWatchApp.DataServices.Repositories;
using SolarWatchApp.JsonProcessor;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from the .env file
DotNetEnv.Env.Load();

// Load configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Services.AddDbContext<SolarWatchContext>();

// Use the connection string from the .env file
var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTIONSTRING");
builder.Services.AddDbContext<SolarWatchContext>(options =>
{
    options.UseSqlServer(connectionString,
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
    });
});


// Add services to the container.
builder.Services.AddHttpClient<SunsetSunriseController>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddTransient<IJsonProcessor, JsonProcessor>();
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();
// CORS Configuration
app.UseCors(options =>
{
    options.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

//app.MapRazorPages();
app.MapControllers();

app.Run();