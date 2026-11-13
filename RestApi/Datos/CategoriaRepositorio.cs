namespace RestApi.Datos;

using MySqlConnector;
using RestApi.Entidades;

/// <summary>
/// Repositorio para la gestión de datos de Categorías.
/// Único lugar con sentencias SQL para la entidad Categoria, utilizando ADO.NET con MySqlConnector.
/// Implementa ICategoriaRepositorio.
/// </summary>
public class CategoriaRepositorio(IConfiguration config) : ICategoriaRepositorio
{
    // Cadena de conexión obtenida de la configuración del sistema (.env o appsettings.json)
    private readonly string _cn = config.GetConnectionString("Restaurante")!;

    /// <summary>
    /// Consulta el catálogo completo de categorías ordenadas alfabéticamente por su nombre.
    /// </summary>
    public async Task<List<Categoria>> ObtenerTodasAsync()
    {
        var lista = new List<Categoria>();
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        await using var cmd = new MySqlCommand(
            "SELECT Id, Nombre FROM Categorias ORDER BY Nombre", cn);

        await using var dr = await cmd.ExecuteReaderAsync();
        while (await dr.ReadAsync())
        {
            lista.Add(new Categoria
            {
                Id = dr.GetInt32(0),
                Nombre = dr.GetString(1)
            });
        }
        return lista;
    }

    /// <summary>
    /// Busca y obtiene una categoría específica según su identificador único.
    /// </summary>
    public async Task<Categoria?> ObtenerPorIdAsync(int id)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        await using var cmd = new MySqlCommand(
            "SELECT Id, Nombre FROM Categorias WHERE Id = @id", cn);
        cmd.Parameters.AddWithValue("@id", id);

        await using var dr = await cmd.ExecuteReaderAsync();
        if (!await dr.ReadAsync()) return null;

        return new Categoria
        {
            Id = dr.GetInt32(0),
            Nombre = dr.GetString(1)
        };
    }

    /// <summary>
    /// Registra una nueva categoría en la base de datos de manera parametrizada.
    /// </summary>
    public async Task<int> AgregarAsync(Categoria c)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        await using var cmd = new MySqlCommand(
            "INSERT INTO Categorias (Nombre) VALUES (@nombre)", cn);
        cmd.Parameters.AddWithValue("@nombre", c.Nombre);

        await cmd.ExecuteNonQueryAsync();
        return (int)cmd.LastInsertedId; // Retorna el Id autogenerado por MySQL
    }

    /// <summary>
    /// Actualiza el nombre de una categoría existente.
    /// </summary>
    public async Task<int> ActualizarAsync(Categoria c)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        await using var cmd = new MySqlCommand(
            "UPDATE Categorias SET Nombre = @nombre WHERE Id = @id", cn);
        cmd.Parameters.AddWithValue("@nombre", c.Nombre);
        cmd.Parameters.AddWithValue("@id", c.Id);

        return await cmd.ExecuteNonQueryAsync(); // Devuelve cantidad de filas afectadas
    }

    /// <summary>
    /// Elimina físicamente una categoría de la base de datos si no tiene platos relacionados.
    /// </summary>
    public async Task<int> EliminarAsync(int id)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        await using var cmd = new MySqlCommand(
            "DELETE FROM Categorias WHERE Id = @id", cn);
        cmd.Parameters.AddWithValue("@id", id);

        return await cmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Cuenta cuántos platos están asociados a una categoría dada.
    /// Utilizado por la capa de negocio para validar la integridad referencial antes de eliminar.
    /// </summary>
    public async Task<int> ContarPlatosAsync(int id)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        await using var cmd = new MySqlCommand(
            "SELECT COUNT(*) FROM Platos WHERE CategoriaId = @id", cn);
        cmd.Parameters.AddWithValue("@id", id);

        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }
}
