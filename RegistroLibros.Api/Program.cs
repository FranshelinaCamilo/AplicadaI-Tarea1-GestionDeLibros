using Microsoft.EntityFrameworkCore;
using RegistroLibros.Data.Context;
using RegistroLibros.Services.Services;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SqlConStr");

builder.Services.AddDbContextFactory<BibliotecaBdContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<EstudiantesService>();
builder.Services.AddScoped<LibrosService>();
builder.Services.AddScoped<PrestamosService>();


builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
