namespace RestApi.Datos;

using RestApi.Entidades;

/// <summary>
/// Contrato para el acceso a datos de productos generales en memoria o almacén.
/// </summary>
public interface IProductoRepositorio
{
    /// <summary>
    /// Obtiene todos los productos registrados.
    /// </summary>
    /// <returns>Lista completa de productos.</returns>
    List<Producto> ObtenerTodos();
}
