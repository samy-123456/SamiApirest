namespace RestApi.Negocio;

using RestApi.Datos;
using RestApi.Entidades;

/// <summary>
/// Capa de negocio para productos de inventario.
/// Depende de la interfaz IProductoRepositorio.
/// </summary>
public class ProductoServicio(IProductoRepositorio repo)
{
    /// <summary>
    /// Retorna los productos que tienen existencia en stock mayor a cero.
    /// </summary>
    public List<Producto> ListarDisponibles() =>
        repo.ObtenerTodos().Where(p => p.Stock > 0).ToList();
}
