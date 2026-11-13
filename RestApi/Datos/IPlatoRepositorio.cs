namespace RestApi.Datos;

using RestApi.Entidades;

/// <summary>
/// Contrato para el acceso a datos de los platos del restaurante.
/// Define las operaciones CRUD y de consulta sobre la tabla 'Platos'.
/// </summary>
public interface IPlatoRepositorio
{
    /// <summary>
    /// Obtiene todos los platos que se encuentran activos/disponibles (Disponible = 1),
    /// ordenados por nombre de categoría y nombre del plato.
    /// </summary>
    /// <returns>Lista de platos activos con el nombre de su categoría asociada.</returns>
    Task<List<Plato>> ObtenerTodosAsync();

    /// <summary>
    /// Obtiene un plato activo por su identificador primario.
    /// </summary>
    /// <param name="id">Identificador único del plato.</param>
    /// <returns>El plato si existe y está disponible, o null en caso contrario.</returns>
    Task<Plato?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Obtiene todos los platos activos pertenecientes a una categoría específica.
    /// </summary>
    /// <param name="categoriaId">Identificador de la categoría.</param>
    /// <returns>Lista de platos pertenecientes a dicha categoría.</returns>
    Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId);

    /// <summary>
    /// Registra un nuevo plato en la base de datos con estado inicial disponible.
    /// </summary>
    /// <param name="p">Entidad plato con los datos a insertar.</param>
    /// <returns>El ID autogenerado del plato insertado.</returns>
    Task<int> AgregarAsync(Plato p);

    /// <summary>
    /// Actualiza la información de un plato existente que esté activo.
    /// </summary>
    /// <param name="p">Entidad plato con los nuevos valores.</param>
    /// <returns>El número de filas afectadas (1 si se actualizó, 0 si no existía o no estaba disponible).</returns>
    Task<int> ActualizarAsync(Plato p);

    /// <summary>
    /// Realiza el borrado lógico de un plato cambiando Disponible = 0.
    /// Nunca ejecuta una sentencia DELETE física para preservar el historial.
    /// </summary>
    /// <param name="id">Identificador del plato a desactivar.</param>
    /// <returns>El número de filas afectadas (1 si se desactivó, 0 si no existía o ya estaba inactivo).</returns>
    Task<int> EliminarAsync(int id);
}
