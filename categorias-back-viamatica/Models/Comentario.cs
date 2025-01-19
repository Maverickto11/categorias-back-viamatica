namespace categorias_back_viamatica.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public int UsuarioId { get; set; }  // Relación con el usuario que hizo el comentario
        public Usuario Usuario { get; set; }  // Relación con el usuario (opcional para la consulta)
        public int PublicacionId { get; set; }  // Relación con la publicación a la que pertenece el comentario
        public Publicacion Publicacion { get; set; }
    }

}
