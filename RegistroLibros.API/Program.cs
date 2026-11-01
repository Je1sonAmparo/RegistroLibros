using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using RegistroLibros.Data.Context;
using RegistroLibros.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var constr = builder.Configuration.GetConnectionString("SqlConStr");

builder.Services.AddDbContextFactory<Contexto>(options =>
    options.UseSqlServer(constr));
builder.Services.AddTransient<EstudiantesService>();
builder.Services.AddTransient<LibrosService>();
builder.Services.AddTransient<PrestamosService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
