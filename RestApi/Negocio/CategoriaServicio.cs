namespace RestApi.Negocio;

using RestApi.Datos;
using RestApi.Entidades;

/// <summary>
/// Capa de negocio para la administración de Categorías.
/// Aplica validaciones de formato e integridad referencial antes de delegar a la persistencia.
/// Depende de la interfaz ICategoriaRepositorio (DIP).
/// </summary>
public class CategoriaServicio(ICategoriaRepositorio repo)
{
    /// <summary>
    /// Consulta el listado de todas las categorías disponibles.
    /// </summary>
    public Task<List<Categoria>> ObtenerTodasAsync() => repo.ObtenerTodasAsync();

    /// <summary>
    /// Obtiene una categoría por su identificador.
    /// Lanza KeyNotFoundException (HTTP 404) si no existe.
    /// </summary>
    public async Task<Categoria> ObtenerPorIdAsync(int id)
    {
        var categoria = await repo.ObtenerPorIdAsync(id);
        if (categoria is null)
            throw new KeyNotFoundException($"No existe la categoría con Id {id}."); // 404
        return categoria;
    }

    /// <summary>
    /// Valida y registra una nueva categoría.
    /// </summary>
    public async Task<int> AgregarAsync(Categoria c)
    {
        Validar(c);
        return await repo.AgregarAsync(c);
    }

    /// <summary>
    /// Valida y actualiza el nombre de una categoría existente.
    /// Lanza KeyNotFoundException (HTTP 404) si no existe.
    /// </summary>
    public async Task ActualizarAsync(Categoria c)
    {
        Validar(c);
        var filas = await repo.ActualizarAsync(c);
        if (filas == 0)
            throw new KeyNotFoundException($"No existe la categoría con Id {c.Id}."); // 404
    }

    /// <summary>
    /// Elimina una categoría si no contiene platos vinculados.
    /// Lanza InvalidOperationException (HTTP 409 Conflicto) si tiene platos asociados.
    /// Lanza KeyNotFoundException (HTTP 404) si la categoría no existe.
    /// </summary>
    public async Task EliminarAsync(int id)
    {
        // Regla de integridad de negocio: no se puede eliminar una categoría si tiene platos asociados
        var platos = await repo.ContarPlatosAsync(id);
        if (platos > 0)
            throw new InvalidOperationException(
                $"La categoría {id} tiene {platos} plato(s) asociado(s). Reasígnalos o elimínalos antes de continuar."); // 409

        var filas = await repo.EliminarAsync(id);
        if (filas == 0)
            throw new KeyNotFoundException($"No existe la categoría con Id {id}."); // 404
    }

    /// <summary>
    /// Reglas de validación para la entidad Categoria:
    /// - El nombre es obligatorio.
    /// - El nombre no puede superar los 50 caracteres.
    /// </summary>
    private static void Validar(Categoria c)
    {
        if (string.IsNullOrWhiteSpace(c.Nombre))
            throw new ArgumentException("El nombre de la categoría es obligatorio."); // 400

        if (c.Nombre.Length > 50)
            throw new ArgumentException("El nombre no puede superar los 50 caracteres."); // 400

        c.Nombre = c.Nombre.Trim();
    }
}
