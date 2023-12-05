using brewlogsMinimalApi.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using brewlogsMinimalApi.Mappers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configuration
new ConfigurationBuilder()
    .SetBasePath(Environment.CurrentDirectory)
    .AddJsonFile("appsettings.Development.json")
    .Build();

// Connection String
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Data
builder.Services.AddDbContext<BrewlogsDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", corsBuilder =>
    {
        // React Default: Port 3000
        corsBuilder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
    options.AddPolicy("ProdCors", corsBuilder =>
    {
        // Production Frontend
        corsBuilder.WithOrigins("https://brewlogger.rogeliomitre.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Controllers
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();