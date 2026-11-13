namespace RestApi.Datos;

using RestApi.Entidades;

/// <summary>
/// Repositorio en memoria para productos generales.
/// Implementa IProductoRepositorio.
/// </summary>
public class ProductoRepositorio : IProductoRepositorio
{
    private static readonly List<Producto> _datos = new()
    {
        new Producto { Id = 1, Nombre = "Mouse", Precio = 12.50m, Stock = 20 },
        new Producto { Id = 2, Nombre = "Teclado", Precio = 25m, Stock = 0 },
        new Producto { Id = 3, Nombre = "Monitor", Precio = 180m, Stock = 5 }
    };

    /// <summary>
    /// Retorna la lista de todos los productos en memoria.
    /// </summary>
    public List<Producto> ObtenerTodos() => _datos;
}
