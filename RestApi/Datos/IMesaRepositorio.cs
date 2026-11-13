using RestApi.Entidades;

namespace RestApi.Datos
{
    public interface IMesaRepositorio
    {
        public Task<List<Mesa>> ObtenerTodasAsync();
        public Task<Mesa?> ObtenerPorIdAsync(int id);
        public Task<bool> ExisteNumeroAsync(int numero,
                                         int idExcluir);
        public  Task<int> AgregarAsync(Mesa m);
        public  Task<bool> ActualizarAsync(Mesa m);
        public Task<bool> EliminarAsync(int id);


    }
}
