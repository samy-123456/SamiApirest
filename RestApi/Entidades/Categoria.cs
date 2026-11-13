namespace RestApi.Entidades;

/// <summary>
/// Representa una categoría en el menú del restaurante (ej. Entradas, Platos Fuertes, Bebidas, Postres).
/// Entidad de tipo POCO sin lógica de negocio.
/// </summary>
public class Categoria
{
    /// <summary>
    /// Identificador único de la categoría (clave primaria autoincremental).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre descriptivo de la categoría. Máximo 60 caracteres.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;
}
