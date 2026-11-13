namespace RestApi.Datos;

using RestApi.Entidades;

/// <summary>
/// Contrato para el acceso a datos de las categorías del menú.
/// Define las operaciones CRUD y validación de relaciones sobre la tabla 'Categorias'.
/// </summary>
public interface ICategoriaRepositorio
{
    /// <summary>
    /// Obtiene el catálogo completo de categorías ordenadas alfabéticamente por su nombre.
    /// </summary>
    /// <returns>Lista de todas las categorías registradas.</returns>
    Task<List<Categoria>> ObtenerTodasAsync();

    /// <summary>
    /// Busca y obtiene una categoría específica según su identificador único.
    /// </summary>
    /// <param name="id">Identificador único de la categoría.</param>
    /// <returns>La entidad de la categoría o null si no fue encontrada.</returns>
    Task<Categoria?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Inserta una nueva categoría en la base de datos.
    /// </summary>
    /// <param name="c">Entidad categoría con el nombre asignado.</param>
    /// <returns>El ID autogenerado asignado por la base de datos.</returns>
    Task<int> AgregarAsync(Categoria c);

    /// <summary>
    /// Modifica los datos de una categoría existente.
    /// </summary>
    /// <param name="c">Entidad categoría con su ID y nombre actualizados.</param>
    /// <returns>Número de registros afectados (1 si se actualizó, 0 si no existe).</returns>
    Task<int> ActualizarAsync(Categoria c);

    /// <summary>
    /// Elimina físicamente una categoría por su identificador.
    /// Debe verificarse previamente que no tenga platos asociados.
    /// </summary>
    /// <param name="id">Identificador de la categoría a eliminar.</param>
    /// <returns>Número de registros afectados (1 si se eliminó, 0 si no existía).</returns>
    Task<int> EliminarAsync(int id);

    /// <summary>
    /// Cuenta la cantidad de platos (activos o históricos) asociados a una categoría.
    /// Se utiliza como regla de negocio para evitar eliminar categorías en uso.
    /// </summary>
    /// <param name="id">Identificador de la categoría a consultar.</param>
    /// <returns>Número total de platos vinculados a la categoría.</returns>
    Task<int> ContarPlatosAsync(int id);
}
