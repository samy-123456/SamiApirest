namespace RestApi.Datos;

using MySqlConnector;
using RestApi.Entidades;

public class PlatoRepositorio(IConfiguration config)
{
    private readonly string _cn = config.GetConnectionString("Restaurante")!;



    public async Task<int> AgregarAsync(Plato p)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new MySqlCommand(
            @"INSERT INTO Platos (Nombre, Descripcion, Precio, CategoriaId)
              VALUES (@nombre, @desc, @precio, @cat)", cn);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@cat", p.CategoriaId);
        await cmd.ExecuteNonQueryAsync();
        return (int)cmd.LastInsertedId;
    }
    //
    public async Task<int> ActualizarAsync(Plato p)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new MySqlCommand(
            "UPDATE Platos SET Nombre = @nombre,Descripcion=@desc, Precio = @precio, CategoriaId = @cat " +
            "WHERE Id = @id AND Disponible = 1", cn);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@cat", p.CategoriaId);
        cmd.Parameters.AddWithValue("@id", p.Id);
        return await cmd.ExecuteNonQueryAsync();
        
    }


    //

    public async Task<int>EliminarAsync(int id )

    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new MySqlCommand(
            "DELET FROM Platos WHERE Id =@id", cn);
        cmd.Parameters.AddWithValue("@id", id);
        return await cmd.ExecuteNonQueryAsync();


    }
    private const string SelectConCategoria = @"
        SELECT p.Id, p.Nombre, p.Descripcion, p.Precio, p.CategoriaId, c.Nombre
        FROM Platos p
        INNER JOIN Categorias c ON c.Id = p.CategoriaId";

    public Task<List<Plato>> ObtenerTodosAsync() =>
        LeerAsync(SelectConCategoria + " WHERE p.Disponible = 1 ORDER BY c.Nombre, p.Nombre");

    public async Task<Plato?> ObtenerPorIdAsync(int id) =>
        (await LeerAsync(SelectConCategoria + " WHERE p.Id = @id", ("@id", id))).FirstOrDefault();

    public Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId) =>
        LeerAsync(SelectConCategoria + " WHERE p.CategoriaId = @cat AND p.Disponible = 1 ORDER BY p.Nombre",
                  ("@cat", categoriaId));

    private async Task<List<Plato>> LeerAsync(string sql, params (string nombre, object valor)[] parametros)
    {
        var lista = new List<Plato>();
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new MySqlCommand(sql, cn);
        foreach (var (nombre, valor) in parametros)
            cmd.Parameters.AddWithValue(nombre, valor);
        await using var dr = await cmd.ExecuteReaderAsync();
        while (await dr.ReadAsync())
            lista.Add(new Plato
            {
                Id = dr.GetInt32(0),
                Nombre = dr.GetString(1),
                Descripcion = dr.IsDBNull(2) ? "" : dr.GetString(2),
                Precio = dr.GetDecimal(3),
                CategoriaId = dr.GetInt32(4),
                Categoria = dr.GetString(5)
            });
        return lista;
    }
}