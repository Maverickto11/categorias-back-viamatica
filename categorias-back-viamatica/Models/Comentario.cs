namespace categorias_back_viamatica.Models
{
    public class Comentario
    {
        public int ComentarioId { get; set; } // ID único del comentario
        public int PublicacionId { get; set; } // ID de la publicación en la que se hizo el comentario
        public int UsuarioId { get; set; } // ID del usuario que hizo el comentario
        public string Contenido { get; set; } // Contenido del comentario
        public DateTime FechaCreacion { get; set; } // Fecha en la que se hizo el comentario
        public Publicacion Publicacion { get; set; } // Relación con la clase Publicacion
        public Usuario Usuario { get; set; } // Relación con la clase Usuario
    }

}
