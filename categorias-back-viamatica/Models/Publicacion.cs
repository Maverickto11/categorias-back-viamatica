namespace categorias_back_viamatica.Models
{
    public class Publicacion
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public int? UsuarioId { get; set; }  // Relación con el Usuario
        public Usuario? Usuario { get; set; }  // Navegación a Usuario

        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }

}
