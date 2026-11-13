using Microsoft.EntityFrameworkCore;
using RestApi.Datos;
using RestApi.Entidades;

namespace RestApi.Negocio
{
    public class MesaServicio(IMesaRepositorio repo)
    {
        public Task<List<Mesa>> ListarAsync() =>
        repo.ObtenerTodasAsync();

        public async Task<Mesa> ObtenerAsync(int id) =>
            await repo.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException(
                   $"No existe la mesa {id}.");

        public async Task<int> CrearAsync(Mesa m)
        {
            await ValidarAsync(m);
            return await repo.AgregarAsync(m);
        }

        public async Task ActualizarAsync(Mesa m)
        {
            await ValidarAsync(m);
            if (!await repo.ActualizarAsync(m))
                throw new KeyNotFoundException(
                    $"No existe la mesa {m.Id}.");
        }

        public async Task EliminarAsync(int id)
        {
            try
            {
                if (!await repo.EliminarAsync(id))
                    throw new KeyNotFoundException(
                        $"No existe la mesa {id}.");
            }
            catch (DbUpdateException)
            {   // tiene pedidos
                throw new InvalidOperationException(
                    $"La mesa {id} tiene pedidos.");
            }
        }

        private async Task ValidarAsync(Mesa m)
        {
            if (m.Numero <= 0) throw new ArgumentException(
                "El número de mesa debe ser mayor a 0.");
            if (m.Capacidad is < 1 or > 20) throw new ArgumentException(
                "La capacidad va de 1 a 20 personas.");
            if (await repo.ExisteNumeroAsync(m.Numero, m.Id))
                throw new InvalidOperationException(
                    $"Ya existe la mesa {m.Numero}.");
        }


    }
}
