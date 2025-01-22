using categorias_back_viamatica.Data;
using categorias_back_viamatica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace categorias_back_viamatica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PublicacionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PublicacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("todas")]
        public IActionResult GetTodasPublicaciones()
        {
            var publicaciones = _context.Publicaciones
                .Include(p => p.Usuario) 
                .Select(p => new
                {
                    p.Id,
                    p.Titulo,
                    p.Contenido,
                    Usuario = p.Usuario.Correo 
                })
                .ToList();

            return Ok(publicaciones);
        }



        [HttpGet("categoria/{categoriaId}")]
        [Authorize]  
        public IActionResult GetPublicacionesPorCategoria(int categoriaId)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value); 

            var publicaciones = _context.Publicaciones
                .Include(p => p.Usuario)
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == categoriaId && p.UsuarioId == usuarioId) 
                .Select(p => new
                {
                    p.Id,
                    p.Titulo,
                    p.Contenido,
                    Usuario = p.Usuario.Correo,  
                    Categoria = p.Categoria.Nombre 
                })
                .ToList();

            if (publicaciones.Count == 0)
            {
                return NotFound(new { mensaje = "No se encontraron publicaciones para esta categoría." });
            }

            return Ok(publicaciones);
        }



        [HttpPost]
        public IActionResult CreatePublicacion([FromBody] Publicacion model)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            if (model.CategoriaId == 0)
            {
                return BadRequest("El ID de la categoría es necesario.");
            }

            var categoria = _context.Categorias.Find(model.CategoriaId);
            if (categoria == null)
            {
                return BadRequest("La categoría especificada no existe.");
            }

            model.UsuarioId = usuarioId;

            model.CategoriaId = categoria.Id;

            _context.Publicaciones.Add(model);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPublicacionesPorCategoria), new { id = model.Id }, model);
        }



        [HttpPut("{id}")]
        public IActionResult EditPublicacion(int id, [FromBody] Publicacion model)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            var publicacion = _context.Publicaciones.SingleOrDefault(p => p.Id == id);

            if (publicacion == null)
            {
                return NotFound(new { mensaje = "Publicación no encontrada" });
            }

            if (publicacion.UsuarioId != usuarioId)
            {
                return Unauthorized(new { mensaje = "No tienes permiso para editar esta publicación" });
            }

            publicacion.Titulo = model.Titulo;
            publicacion.Contenido = model.Contenido;

            _context.SaveChanges();

            return Ok(new { mensaje = "Publicación actualizada correctamente", publicacion });
        }


        [HttpDelete("{id}")]
        public IActionResult DeletePublicacion(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            var publicacion = _context.Publicaciones.SingleOrDefault(p => p.Id == id);

            if (publicacion == null)
            {
                return NotFound(new { mensaje = "Publicación no encontrada" });
            }

            if (publicacion.UsuarioId != usuarioId)
            {
                return Unauthorized(new { mensaje = "No tienes permiso para eliminar esta publicación" });
            }

            _context.Publicaciones.Remove(publicacion);
            _context.SaveChanges();

            return Ok(new { mensaje = "Publicación eliminada correctamente" });
        }
    }
}

