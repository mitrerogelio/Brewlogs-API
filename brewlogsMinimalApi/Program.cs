using brewlogsMinimalApi.Data;
using brewlogsMinimalApi.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BrewlogsDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/brewlogs", async (BrewlogsDbContext dbContext) =>
{
    var brewlogs = await dbContext.Brewlogs.ToListAsync();
    return Results.Ok(brewlogs);
});

app.MapGet("/brewlogs/{id}", async (int id, BrewlogsDbContext dbContext) =>
{
    var brewlog = await dbContext.Brewlogs.FindAsync(id);
    return brewlog == null ? Results.NotFound() : Results.Ok(brewlog);
});

app.MapPost("/brewlogs", async (Brewlog brewlog, BrewlogsDbContext dbContext) =>
{
    dbContext.Brewlogs.Add(brewlog);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/brewlogs/{brewlog.Id}", brewlog);
});

app.MapPut("/brewlogs/{id}", async (int id, Brewlog updatedBrewlog, BrewlogsDbContext dbContext) =>
{
    var existingBrewlog = await dbContext.Brewlogs.FindAsync(id);
    if (existingBrewlog == null)
    {
        return Results.NotFound();
    }

    existingBrewlog.CoffeeName = updatedBrewlog.CoffeeName;
    existingBrewlog.Dose = updatedBrewlog.Dose;
    existingBrewlog.Grind = updatedBrewlog.Grind;
    existingBrewlog.BrewRatio = updatedBrewlog.BrewRatio;
    existingBrewlog.Roast = updatedBrewlog.Roast;
    existingBrewlog.BrewerUsed = updatedBrewlog.BrewerUsed;

    await dbContext.SaveChangesAsync();
    return Results.Ok(existingBrewlog);
});

app.MapDelete("/brewlogs/{id}", async (int id, BrewlogsDbContext dbContext) =>
{
    var brewlog = await dbContext.Brewlogs.FindAsync(id);
    if (brewlog == null)
    {
        return Results.NotFound();
    }

    dbContext.Brewlogs.Remove(brewlog);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();