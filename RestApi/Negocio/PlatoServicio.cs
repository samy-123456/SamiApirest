namespace RestApi.Negocio;

using RestApi.Datos;
using RestApi.Entidades;

/// <summary>
/// Capa de negocio para la gestión de platos.
/// Contiene las reglas de validación y flujos de negocio del restaurante.
/// Depende exclusivamente de abstracciones (IPlatoRepositorio, ICategoriaRepositorio) cumpliendo DIP.
/// </summary>
public class PlatoServicio(IPlatoRepositorio repo, ICategoriaRepositorio categorias)
{
    /// <summary>
    /// Obtiene la lista completa de platos activos disponibles.
    /// </summary>
    public Task<List<Plato>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

    /// <summary>
    /// Obtiene un plato activo por su identificador.
    /// Lanza KeyNotFoundException (HTTP 404) si no existe o fue dado de baja.
    /// </summary>
    public async Task<Plato> ObtenerPorIdAsync(int id)
    {
        var plato = await repo.ObtenerPorIdAsync(id);
        if (plato is null)
            throw new KeyNotFoundException($"No existe el plato con Id {id}."); // Manejado como 404
        return plato;
    }

    /// <summary>
    /// Obtiene los platos asociados a una categoría específica, validando previamente que la categoría exista.
    /// Lanza KeyNotFoundException (HTTP 404) si la categoría no existe.
    /// </summary>
    public async Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId)
    {
        if (await categorias.ObtenerPorIdAsync(categoriaId) is null)
            throw new KeyNotFoundException($"No existe la categoría {categoriaId}."); // Manejado como 404
        return await repo.ObtenerPorCategoriaAsync(categoriaId);
    }

    /// <summary>
    /// Valida las reglas de negocio y registra un nuevo plato.
    /// </summary>
    public async Task<int> AgregarAsync(Plato p)
    {
        await ValidarAsync(p);
        return await repo.AgregarAsync(p);
    }

    /// <summary>
    /// Valida y actualiza los datos de un plato existente.
    /// Lanza KeyNotFoundException (HTTP 404) si el plato no existe o no está activo.
    /// </summary>
    public async Task ActualizarAsync(Plato p)
    {
        await ValidarAsync(p);
        var filas = await repo.ActualizarAsync(p);
        if (filas == 0)
            throw new KeyNotFoundException($"No existe el plato con Id {p.Id} o está inactivo."); // Manejado como 404
    }

    /// <summary>
    /// Ejecuta el borrado lógico de un plato.
    /// Lanza KeyNotFoundException (HTTP 404) si el plato no existe o ya estaba inactivo.
    /// </summary>
    public async Task EliminarAsync(int id)
    {
        var filas = await repo.EliminarAsync(id);
        if (filas == 0)
            throw new KeyNotFoundException($"No existe el plato con Id {id} o ya fue desactivado."); // Manejado como 404
    }

    /// <summary>
    /// Reglas de validación para la entidad Plato:
    /// 1. El nombre es obligatorio y no debe superar 100 caracteres.
    /// 2. El precio debe ser estrictamente mayor a 0.
    /// 3. La categoría asignada debe existir en la base de datos.
    /// </summary>
    private async Task ValidarAsync(Plato p)
    {
        if (string.IsNullOrWhiteSpace(p.Nombre))
            throw new ArgumentException("El nombre del plato es obligatorio."); // Manejado como 400

        if (p.Nombre.Length > 100)
            throw new ArgumentException("El nombre no puede superar los 100 caracteres."); // Manejado como 400

        if (p.Precio <= 0)
            throw new ArgumentException("El precio del plato debe ser mayor a 0."); // Manejado como 400

        if (await categorias.ObtenerPorIdAsync(p.CategoriaId) is null)
            throw new ArgumentException($"La categoría con Id {p.CategoriaId} no existe."); // Manejado como 400

        p.Nombre = p.Nombre.Trim();
        p.Descripcion = (p.Descripcion ?? string.Empty).Trim();
    }
}
