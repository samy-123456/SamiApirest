namespace RestApi.Servicios;

using MySqlConnector;
using RestApi.Datos;
using RestApi.Entidades;

public class PlatoServicio(PlatoRepositorio repo, CategoriaRepositorio categorias)
{
    public Task<List<Plato>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

    public async Task<Plato> ObtenerPorIdAsync(int id)
    {
        var plato = await repo.ObtenerPorIdAsync(id);
        if (plato is null)
            throw new KeyNotFoundException($"No existe el plato {id}.");                  // 404
        return plato;
    }

    public async Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId)
    {
        if (await categorias.ObtenerPorIdAsync(categoriaId) is null)
            throw new KeyNotFoundException($"No existe la categoría {categoriaId}.");     // 404
        return await repo.ObtenerPorCategoriaAsync(categoriaId);
    }

    public async Task<int> AgregarAsync(Plato p)
    {
        await ValidarAsync(p);
        return await repo.AgregarAsync(p);
    }
    public async Task ActualizarAsync(Plato p)
    {
        await ValidarAsync(p);
        var filas = await repo.ActualizarAsync(p);
        if (filas == 0)
            throw new KeyNotFoundException($"No existe el plato {p.Id}.");                // 404
    }
    public async Task EliminarAsync(int id)
    {
        var filas = await repo.EliminarAsync(id);
        if (filas == 0)
            throw new KeyNotFoundException($"No existe el plato {id}.");                  // 404
    }
    private async Task ValidarAsync(Plato p)
    {
        if (string.IsNullOrWhiteSpace(p.Nombre))
            throw new ArgumentException("El nombre del plato es obligatorio.");          // 400
        if (p.Nombre.Length > 100)
            throw new ArgumentException("El nombre no puede superar los 100 caracteres.");
        if (p.Precio <= 0)
            throw new ArgumentException("El precio debe ser mayor a 0.");
        if (await categorias.ObtenerPorIdAsync(p.CategoriaId) is null)
            throw new ArgumentException($"La categoría {p.CategoriaId} no existe.");      // 400
        p.Nombre = p.Nombre.Trim();
        p.Descripcion = (p.Descripcion ?? "").Trim();
    }

}