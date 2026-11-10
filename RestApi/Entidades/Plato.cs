namespace RestApi.Entidades
{
    public class Plato
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }

        // Solo para mostrar: se llena con el JOIN a Categorias
        public string Categoria { get; set; } = "";
    }
}
