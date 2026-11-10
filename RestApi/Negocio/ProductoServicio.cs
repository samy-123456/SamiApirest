using RestApi.Datos;
using RestApi.Entidades;

namespace RestApi.Negocio
{
    public class ProductoServicio
    {
        private readonly ProductoRepositorio _repo;

 
        public ProductoServicio(ProductoRepositorio repo)
        {
            _repo = repo;
        }
        public List<Producto> ListarDisponibles() =>
            _repo.ObtenerTodos().Where(p => p.Stock > 0).ToList();
    }
}
