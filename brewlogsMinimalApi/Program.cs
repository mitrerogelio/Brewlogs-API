using Microsoft.EntityFrameworkCore;
using brewlogsMinimalApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using var db = new DataContext();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Routes
app.MapGet("/logs", async (DataContext db) =>
{
  try
  {
    var brewlogs = await db.Brewlogs.ToListAsync();
    return Results.Ok(brewlogs);
  }
  catch (InvalidOperationException ex)
  {
    return Results.Problem("An error occurred while accessing the database.");
  }
});


app.Run();
