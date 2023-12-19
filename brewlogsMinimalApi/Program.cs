using System.Text;
using brewlogsMinimalApi.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

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

string? tokenKeyString = builder.Configuration["AppSettings:TokenKey"];
string? validIssuer = builder.Configuration["Authentication:Schemes:Bearer:ValidIssuer"];
IConfigurationSection validAudiencesSection =
    builder.Configuration.GetSection("Authentication:Schemes:Bearer:ValidAudiences");
string[] validAudiences = validAudiencesSection.Get<string[]>() ?? Array.Empty<string>();

Console.WriteLine($"Token Key: {(tokenKeyString ?? "Token key not found")}");
Console.WriteLine($"Valid Issuer: {(string.IsNullOrEmpty(validIssuer) ? "Issuer not found" : validIssuer)}");
if (validAudiences.Any())
{
    foreach (string audience in validAudiences)
    {
        Console.WriteLine($"Valid Audience: {audience}");
    }
}
else
{
    Console.WriteLine("No valid audiences found");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString ?? "")),
            ValidateIssuer = !string.IsNullOrEmpty(validIssuer),
            ValidIssuer = validIssuer,
            ValidateAudience = validAudiences.Any(),
            ValidAudiences = validAudiences
        };
    });

// Controllers
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpContextAccessor();
// builder.Services.AddScoped<IBrewlogRepository, BrewlogRepository>();
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