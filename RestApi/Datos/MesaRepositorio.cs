using Microsoft.EntityFrameworkCore;
using RestApi.Entidades;

namespace RestApi.Datos
{
   
    public class MesaRepositorio(RestauranteContext db)
    : IMesaRepositorio
    {
        public Task<List<Mesa>> ObtenerTodasAsync() =>
            db.Mesas.AsNoTracking()
              .OrderBy(m => m.Numero).ToListAsync();

        public Task<Mesa?> ObtenerPorIdAsync(int id) =>
            db.Mesas.FindAsync(id).AsTask();

        public Task<bool> ExisteNumeroAsync(int numero,
                                             int idExcluir) =>
            db.Mesas.AnyAsync(m => m.Numero == numero
                                && m.Id != idExcluir);

        public async Task<int> AgregarAsync(Mesa m)
        {
            m.Id = 0;
            db.Mesas.Add(m);
            await db.SaveChangesAsync();
            return m.Id;           // EF Core ya lo llenó
        }
        public async Task<bool> ActualizarAsync(Mesa m)
        {
            var actual = await db.Mesas.FindAsync(m.Id);
            if (actual is null) return false;
            actual.Numero = m.Numero;
            actual.Capacidad = m.Capacidad;
            await db.SaveChangesAsync();  // solo lo que cambió
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var mesa = await db.Mesas.FindAsync(id);
            if (mesa is null) return false;
            db.Mesas.Remove(mesa);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
