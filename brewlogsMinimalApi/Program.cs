using brewlogsMinimalApi.Data;
using brewlogsMinimalApi.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Routes
app.MapGet("/logs", () => { Console.WriteLine("Hello World"); })
    .WithOpenApi();

app.MapGet("/logs/${id}", () => { });

app.MapDelete("/logs/${id}", () => { });

app.MapPut("/logs/${id}", (Brewlog log) => { });

app.Run();
