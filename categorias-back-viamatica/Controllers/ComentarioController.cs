using categorias_back_viamatica.Data;
using categorias_back_viamatica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace categorias_back_viamatica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  
    public class ComentarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComentarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("publicacion/{publicacionId}")]
        public IActionResult GetComentariosByPublicacion(int publicacionId)
        {
            var comentarios = _context.Comentarios
                .Where(c => c.PublicacionId == publicacionId)
                .Include(c => c.Usuario)  
                .Select(c => new
                {
                    c.Id,
                    c.Contenido,
                    Usuario = c.Usuario.Correo,  
                    c.FechaCreacion
                })
                .ToList();

            return Ok(comentarios);
        }

        [HttpPost("{publicacionId}/comentarios")]
        public IActionResult CreateComentario(int publicacionId, [FromBody] JsonElement requestBody)
        {
            if (!requestBody.TryGetProperty("Contenido", out JsonElement contenidoElement) || string.IsNullOrWhiteSpace(contenidoElement.GetString()))
            {
                return BadRequest("El contenido del comentario es obligatorio.");
            }

            string contenido = contenidoElement.GetString();

            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            var publicacion = _context.Publicaciones.Find(publicacionId);
            if (publicacion == null)
            {
                return NotFound($"No se encontró ninguna publicación con el ID {publicacionId}.");
            }

            var comentario = new Comentario
            {
                Contenido = contenido,
                UsuarioId = usuarioId,
                PublicacionId = publicacionId,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Comentarios.Add(comentario);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetComentariosByPublicacion), new { publicacionId = publicacionId }, comentario);
        }

        [HttpPut("{comentarioId}")]
        public IActionResult EditComentario(int comentarioId, [FromBody] JsonElement requestBody)
        {
            if (!requestBody.TryGetProperty("Contenido", out JsonElement contenidoElement) || string.IsNullOrWhiteSpace(contenidoElement.GetString()))
            {
                return BadRequest("El contenido del comentario es obligatorio.");
            }

            string nuevoContenido = contenidoElement.GetString();

            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            var comentario = _context.Comentarios.Find(comentarioId);
            if (comentario == null)
            {
                return NotFound($"No se encontró ningún comentario con el ID {comentarioId}.");
            }

            if (comentario.UsuarioId != usuarioId)
            {
                return Forbid("No tienes permiso para editar este comentario.");
            }

         
            comentario.Contenido = nuevoContenido;
            comentario.FechaCreacion = DateTime.UtcNow; 

            _context.Comentarios.Update(comentario);
            _context.SaveChanges();

            return Ok(comentario);
        }





        [HttpDelete("{id}")]
        public IActionResult DeleteComentario(int id)
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            var comentario = _context.Comentarios.SingleOrDefault(c => c.Id == id);

            if (comentario == null)
            {
                return NotFound(new { mensaje = "Comentario no encontrado" });
            }

            if (comentario.UsuarioId != usuarioId)
            {
                return Unauthorized(new { mensaje = "No tienes permiso para eliminar este comentario" });
            }

            _context.Comentarios.Remove(comentario);
            _context.SaveChanges();

            return Ok(new { mensaje = "Comentario eliminado correctamente" });
        }
    }

}