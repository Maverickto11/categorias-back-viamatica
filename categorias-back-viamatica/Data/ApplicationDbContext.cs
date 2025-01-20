using categorias_back_viamatica.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace categorias_back_viamatica.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Publicacion> Publicaciones { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Agregar categorías predeterminadas
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Deportes", ImagenUrl = "url_de_imagen_deportes" },
                new Categoria { Id = 2, Nombre = "Artes", ImagenUrl = "url_de_imagen_artes" },
                new Categoria { Id = 3, Nombre = "Tecnología", ImagenUrl = "url_de_imagen_tecnologia" },
                new Categoria { Id = 4, Nombre = "Videojuegos", ImagenUrl = "url_de_imagen_videojuegos" },
                new Categoria { Id = 5, Nombre = "Ciencia", ImagenUrl = "url_de_imagen_ciencia" },
                new Categoria { Id = 6, Nombre = "Música", ImagenUrl = "url_de_imagen_musica" },
                new Categoria { Id = 7, Nombre = "Cine", ImagenUrl = "url_de_imagen_cine" },
                new Categoria { Id = 8, Nombre = "Viajes", ImagenUrl = "url_de_imagen_viajes" }
            );
        }

    }

}
