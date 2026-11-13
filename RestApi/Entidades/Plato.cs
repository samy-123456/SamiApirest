namespace RestApi.Entidades;

/// <summary>
/// Representa un ítem del menú (plato, bebida o postre) ofrecido a los comensales.
/// Entidad de tipo POCO sin lógica de negocio.
/// </summary>
public class Plato
{
    /// <summary>
    /// Identificador único del plato (clave primaria autoincremental).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre comercial del plato.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de ingredientes o preparación del plato.
    /// </summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Precio de venta al público en la moneda local.
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// Identificador foráneo que relaciona el plato con una categoría existente.
    /// </summary>
    public int CategoriaId { get; set; }

    /// <summary>
    /// Estado del plato para borrado lógico (1 = disponible/activo, 0 = no disponible/inactivo).
    /// Por defecto es true (1).
    /// </summary>
    public int Disponible { get; set; } = 1;

    /// <summary>
    /// Nombre de la categoría asociada (campo poblado mediante JOIN en consultas SQL para visualización).
    /// </summary>
    public string Categoria { get; set; } = string.Empty;
}
