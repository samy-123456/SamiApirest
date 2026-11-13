namespace RestApi.Web;

using RestApi.Entidades;
using RestApi.Negocio;

/// <summary>
/// Mapeo de rutas y endpoints para la entidad Categoria usando ASP.NET Core Minimal APIs.
/// Aplica MapGroup("/categorias") y delega la lógica de negocio a CategoriaServicio y PlatoServicio.
/// </summary>
public static class CategoriaEndpoints
{
    /// <summary>
    /// Registra los endpoints HTTP relacionados con la gestión de categorías.
    /// </summary>
    public static void MapCategoriaEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/categorias");

        // GET /categorias - Lista todas las categorías
        grupo.MapGet("/", async (CategoriaServicio s) =>
            Results.Ok(await s.ObtenerTodasAsync()));

        // GET /categorias/{id} - Obtener categoría por ID
        grupo.MapGet("/{id:int}", (int id, CategoriaServicio s) =>
            Respuestas.Manejar(async () => Results.Ok(await s.ObtenerPorIdAsync(id))));

        // GET /categorias/{id}/platos - Relación: todos los platos de una categoría
        grupo.MapGet("/{id:int}/platos", (int id, PlatoServicio s) =>
            Respuestas.Manejar(async () => Results.Ok(await s.ObtenerPorCategoriaAsync(id))));

        // POST /categorias - Crear una categoría (HTTP 201)
        grupo.MapPost("/", (Categoria c, CategoriaServicio s) =>
            Respuestas.Manejar(async () =>
            {
                c.Id = await s.AgregarAsync(c);
                return Results.Created($"/categorias/{c.Id}", c); // 201 Created
            }));

        // PUT /categorias/{id} - Actualizar categoría existente (HTTP 200)
        grupo.MapPut("/{id:int}", (int id, Categoria c, CategoriaServicio s) =>
            Respuestas.Manejar(async () =>
            {
                c.Id = id;
                await s.ActualizarAsync(c);
                return Results.Ok(c);
            }));

        // DELETE /categorias/{id} - Eliminar categoría si no contiene platos asociados (HTTP 204)
        grupo.MapDelete("/{id:int}", (int id, CategoriaServicio s) =>
            Respuestas.Manejar(async () =>
            {
                await s.EliminarAsync(id);
                return Results.NoContent(); // 204 No Content
            }));
    }
}
