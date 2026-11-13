namespace RestApi.Web;

/// <summary>
/// Manejador global centralizado para capturar excepciones del dominio
/// y transformarlas en respuestas HTTP estandarizadas según la convención del proyecto:
/// - ArgumentException         -> HTTP 400 Bad Request
/// - KeyNotFoundException      -> HTTP 404 Not Found
/// - InvalidOperationException -> HTTP 409 Conflict
/// </summary>
public static class Respuestas
{
    /// <summary>
    /// Envuelve la ejecución de un endpoint en un bloque try-catch seguro,
    /// traduciendo errores de negocio a los códigos de estado HTTP apropiados.
    /// </summary>
    public static async Task<IResult> Manejar(Func<Task<IResult>> accion)
    {
        try
        {
            return await accion();
        }
        catch (ArgumentException ex) // Datos de entrada inválidos o faltantes
        {
            return Results.BadRequest(new { error = ex.Message }); // 400
        }
        catch (KeyNotFoundException ex) // Recurso no encontrado o inactivo
        {
            return Results.NotFound(new { error = ex.Message }); // 404
        }
        catch (InvalidOperationException ex) // Conflicto con regla de negocio (ej. borrar categoría con platos)
        {
            return Results.Conflict(new { error = ex.Message }); // 409
        }
        catch (Exception ex) // Errores imprevistos del servidor
        {
            return Results.Problem(detail: ex.Message, statusCode: 500); // 500
        }
    }
}
