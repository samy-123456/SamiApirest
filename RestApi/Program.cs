using RestApi.Datos;
using RestApi.Entidades;
using RestApi.Negocio;
using RestApi.Servicios;
using RestApi.Web;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddScoped<ProductoRepositorio>();
builder.Services.AddScoped<ProductoServicio>();

builder.Services.AddScoped<PlatoRepositorio>();
builder.Services.AddScoped<PlatoServicio>();

builder.Services.AddScoped<CategoriaRepositorio>();
builder.Services.AddScoped<CategoriaServicio>();
var app = builder.Build();

// Productos
app.MapGet("/productos", (ProductoServicio servicio) =>
{
    var productosDisponibles = servicio.ListarDisponibles();
    return Results.Ok(productosDisponibles);
});

app.MapCategoriaEndpoints();
app.MapPlatoEndpoints();



app.Run();