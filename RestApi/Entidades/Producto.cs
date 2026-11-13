namespace RestApi.Entidades;

/// <summary>
/// Representa un producto genérico de inventario con control de existencias.
/// </summary>
public class Producto
{
    /// <summary>
    /// Identificador único del producto.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre comercial del producto.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Precio unitario de venta.
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// Cantidad disponible en stock.
    /// </summary>
    public int Stock { get; set; }
}
