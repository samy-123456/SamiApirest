namespace RestApi.Datos;

using MySqlConnector;
using RestApi.Entidades;

/// <summary>
/// Repositorio para la gestión de datos de Platos.
/// Único lugar con sentencias SQL para la entidad Plato, utilizando ADO.NET con MySqlConnector.
/// Implementa IPlatoRepositorio asegurando la inyección por interfaz.
/// </summary>
public class PlatoRepositorio(IConfiguration config) : IPlatoRepositorio
{
    // Cadena de conexión obtenida de la configuración del sistema (.env o appsettings.json)
    private readonly string _cn = config.GetConnectionString("Restaurante")!;

    // Consulta base con JOIN a la tabla Categorias para obtener el nombre legible de la categoría
    private const string SelectConCategoria = @"
        SELECT p.Id, p.Nombre, p.Descripcion, p.Precio, p.CategoriaId, c.Nombre, p.Disponible
        FROM Platos p
        INNER JOIN Categorias c ON c.Id = p.CategoriaId";

    /// <summary>
    /// Consulta todos los platos activos (Disponible = 1), ordenados por categoría y nombre.
    /// </summary>
    public Task<List<Plato>> ObtenerTodosAsync() =>
        LeerAsync(SelectConCategoria + " WHERE p.Disponible = 1 ORDER BY c.Nombre, p.Nombre");

    /// <summary>
    /// Obtiene un plato activo por su identificador primario.
    /// Retorna null si no existe o si fue dado de baja (Disponible = 0).
    /// </summary>
    public async Task<Plato?> ObtenerPorIdAsync(int id) =>
        (await LeerAsync(SelectConCategoria + " WHERE p.Id = @id AND p.Disponible = 1", ("@id", id))).FirstOrDefault();

    /// <summary>
    /// Obtiene los platos activos que pertenecen a una categoría específica.
    /// </summary>
    public Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId) =>
        LeerAsync(SelectConCategoria + " WHERE p.CategoriaId = @cat AND p.Disponible = 1 ORDER BY p.Nombre",
                  ("@cat", categoriaId));

    /// <summary>
    /// Inserta un nuevo plato en la base de datos con Disponible = 1 por defecto.
    /// Utiliza parámetros SQL para evitar inyecciones maliciosas.
    /// </summary>
    public async Task<int> AgregarAsync(Plato p)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        await using var cmd = new MySqlCommand(
            @"INSERT INTO Platos (Nombre, Descripcion, Precio, CategoriaId, Disponible)
              VALUES (@nombre, @desc, @precio, @cat, 1)", cn);

        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@cat", p.CategoriaId);

        await cmd.ExecuteNonQueryAsync();
        return (int)cmd.LastInsertedId; // Retorna el ID autogenerado en MySQL
    }

    /// <summary>
    /// Actualiza la información de un plato que aún esté activo (Disponible = 1).
    /// </summary>
    public async Task<int> ActualizarAsync(Plato p)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        await using var cmd = new MySqlCommand(
            @"UPDATE Platos 
              SET Nombre = @nombre, Descripcion = @desc, Precio = @precio, CategoriaId = @cat 
              WHERE Id = @id AND Disponible = 1", cn);

        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@cat", p.CategoriaId);
        cmd.Parameters.AddWithValue("@id", p.Id);

        return await cmd.ExecuteNonQueryAsync(); // Devuelve 1 si se actualizó, 0 si no existía o estaba inactivo
    }

    /// <summary>
    /// Borrado lógico: Cambia el flag Disponible = 0.
    /// Regla del proyecto: NUNCA se ejecuta DELETE físico en Platos para mantener integridad de pedidos.
    /// </summary>
    public async Task<int> EliminarAsync(int id)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        await using var cmd = new MySqlCommand(
            "UPDATE Platos SET Disponible = 0 WHERE Id = @id AND Disponible = 1", cn);

        cmd.Parameters.AddWithValue("@id", id);
        return await cmd.ExecuteNonQueryAsync(); // Devuelve 1 si se desactivó, 0 si no existía o ya estaba inactivo
    }

    /// <summary>
    /// Método privado reutilizable para ejecutar consultas SELECT mapeando a la entidad Plato.
    /// Usa siempre parámetros SQL en tuplas para garantizar seguridad contra inyección SQL.
    /// </summary>
    private async Task<List<Plato>> LeerAsync(string sql, params (string nombre, object valor)[] parametros)
    {
        var lista = new List<Plato>();
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        await using var cmd = new MySqlCommand(sql, cn);
        foreach (var (nombre, valor) in parametros)
        {
            cmd.Parameters.AddWithValue(nombre, valor);
        }

        await using var dr = await cmd.ExecuteReaderAsync();
        while (await dr.ReadAsync())
        {
            lista.Add(new Plato
            {
                Id = dr.GetInt32(0),
                Nombre = dr.GetString(1),
                Descripcion = dr.IsDBNull(2) ? "" : dr.GetString(2),
                Precio = dr.GetDecimal(3),
                CategoriaId = dr.GetInt32(4),
                Categoria = dr.GetString(5),
                Disponible = dr.GetInt32(6)
            });
        }
        return lista;
    }
}
