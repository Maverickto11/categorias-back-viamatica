using System.Text.Json.Serialization;

namespace categorias_back_viamatica.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ImagenUrl { get; set; }

        // Relación con publicaciones
        [JsonIgnore]
        public ICollection<Publicacion> Publicaciones { get; set; }
    }

}
