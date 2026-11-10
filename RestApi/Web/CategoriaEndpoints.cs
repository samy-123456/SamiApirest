using RestApi.Entidades;
using RestApi.Negocio;
using RestApi.Servicios;

namespace RestApi.Web
{
    public static class CategoriaEndpoints
    {
        public static void MapCategoriaEndpoints(this WebApplication app)
        {
            var grupo = app.MapGroup("/categorias");   // todas empiezan con /categorias

            grupo.MapGet("/", async (CategoriaServicio s) =>
                Results.Ok(await s.ObtenerTodasAsync()));

            grupo.MapGet("/{id:int}", (int id, CategoriaServicio s) =>
                Respuestas.Manejar(async () => Results.Ok(await s.ObtenerPorIdAsync(id))));

            // La relación: todos los platos de una categoría
            grupo.MapGet("/{id:int}/platos", (int id, PlatoServicio s) =>
                Respuestas.Manejar(async () => Results.Ok(await s.ObtenerPorCategoriaAsync(id))));


            // parteinicial

            ///
            grupo.MapPost("/", (Categoria c, CategoriaServicio s) =>
        Respuestas.Manejar(async () =>
        {
            c.Id = await s.AgregarAsync(c);
            return Results.Created($"/categorias/{c.Id}", c);
        }));

            grupo.MapPut("/{id:int}", (int id, Categoria c, CategoriaServicio s) =>
                Respuestas.Manejar(async () =>
                {
                    c.Id = id;
                    await s.ActualizarAsync(c);
                    return Results.Ok(c);
                }));

            grupo.MapDelete("/{id:int}", (int id, CategoriaServicio s) =>
                Respuestas.Manejar(async () =>
                {
                    await s.EliminarAsync(id);
                    return Results.NoContent();
                }));




            // parte final

        }
      
    }
}
        


    

