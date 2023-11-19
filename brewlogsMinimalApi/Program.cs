using brewlogsMinimalApi.Data;
using brewlogsMinimalApi.Identity;
using brewlogsMinimalApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Environment.CurrentDirectory)
    .AddJsonFile("appsettings.Development.json")
    .Build();

// Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Identity
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

// Data
builder.Services.AddDbContext<BrewlogsDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authentication:ValidIssuer"];
        options.Audience = builder.Configuration["Authentication:ValidAudiences:0"];
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BearerPolicy", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

// Controllers
builder.Services.AddControllers();

builder.Services.AddScoped<JwtService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

// Require Authentication for Endpoints
app.Use(async (context, next) =>
{
    var authResult = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
    if (!authResult.Succeeded)
    {
        context.Response.StatusCode = 401;
        return;
    }

    await next();
});

app.MapControllers();

app.Run();