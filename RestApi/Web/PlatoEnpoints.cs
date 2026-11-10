namespace RestApi.Web;

using RestApi.Entidades;
using RestApi.Negocio;
using RestApi.Servicios;

public static class PlatoEndpoints
{
    public static void MapPlatoEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/platos");

        grupo.MapGet("/", async (PlatoServicio s) =>
            Results.Ok(await s.ObtenerTodosAsync()));

        grupo.MapGet("/{id:int}", (int id, PlatoServicio s) =>
            Respuestas.Manejar(async () => Results.Ok(await s.ObtenerPorIdAsync(id))));

        grupo.MapPost("/", (Plato p, PlatoServicio s) =>
            Respuestas.Manejar(async () =>
            {
                p.Id = await s.AgregarAsync(p);
                return Results.Created($"/platos/{p.Id}", p);
            }));
        grupo.MapPut("/{id:int}", (int id, Plato p, PlatoServicio s) =>
            Respuestas.Manejar(async () =>
            {
                p.Id = id;
                await s.ActualizarAsync(p);
                return Results.Ok(p);
            }));

        grupo.MapDelete("/{id:int}", (int id, PlatoServicio s) =>
            Respuestas.Manejar(async () =>
            {
                await s.EliminarAsync(id);
                return Results.NoContent();      // 204
            }));
    }



}







