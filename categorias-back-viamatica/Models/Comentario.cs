namespace categorias_back_viamatica.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public int? UsuarioId { get; set; } 
        public Usuario? Usuario { get; set; }  
        public int? PublicacionId { get; set; }  
        public Publicacion? Publicacion { get; set; }
        public DateTime FechaCreacion { get; set; }

    }

}
