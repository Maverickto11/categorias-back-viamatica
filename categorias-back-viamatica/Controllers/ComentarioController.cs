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
    [Authorize]  // Requiere autenticación para crear un comentario
    public class ComentarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComentarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener los comentarios de una publicación específica
        [HttpGet("publicacion/{publicacionId}")]
        public IActionResult GetComentariosByPublicacion(int publicacionId)
        {
            var comentarios = _context.Comentarios
                .Where(c => c.PublicacionId == publicacionId)
                .Include(c => c.Usuario)  // Incluye el usuario para mostrar su nombre o correo, por ejemplo
                .Select(c => new
                {
                    c.Id,
                    c.Contenido,
                    Usuario = c.Usuario.Correo,  // Puedes personalizar lo que devuelves aquí
                    c.FechaCreacion
                })
                .ToList();

            return Ok(comentarios);
        }

        // Crear un nuevo comentario
        [HttpPost("{publicacionId}/comentarios")]
        public IActionResult CreateComentario(int publicacionId, [FromBody] JsonElement requestBody)
        {
            // Extrae el contenido del comentario del JSON
            if (!requestBody.TryGetProperty("Contenido", out JsonElement contenidoElement) || string.IsNullOrWhiteSpace(contenidoElement.GetString()))
            {
                return BadRequest("El contenido del comentario es obligatorio.");
            }

            string contenido = contenidoElement.GetString();

            // Obtén el ID del usuario autenticado desde el token JWT
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            // Verifica que la publicación existe
            var publicacion = _context.Publicaciones.Find(publicacionId);
            if (publicacion == null)
            {
                return NotFound($"No se encontró ninguna publicación con el ID {publicacionId}.");
            }

            // Crea un nuevo comentario
            var comentario = new Comentario
            {
                Contenido = contenido,
                UsuarioId = usuarioId,
                PublicacionId = publicacionId,
                FechaCreacion = DateTime.UtcNow
            };

            // Guarda el comentario en la base de datos
            _context.Comentarios.Add(comentario);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetComentariosByPublicacion), new { publicacionId = publicacionId }, comentario);
        }


        // Eliminar un comentario (solo el propietario puede eliminarlo)
        [HttpDelete("{id}")]
        public IActionResult DeleteComentario(int id)
        {
            // Obtén el ID del usuario autenticado desde el token JWT
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            // Busca el comentario en la base de datos
            var comentario = _context.Comentarios.SingleOrDefault(c => c.Id == id);

            if (comentario == null)
            {
                return NotFound(new { mensaje = "Comentario no encontrado" });
            }

            // Verifica si el usuario autenticado es el propietario del comentario
            if (comentario.UsuarioId != usuarioId)
            {
                return Unauthorized(new { mensaje = "No tienes permiso para eliminar este comentario" });
            }

            // Elimina el comentario
            _context.Comentarios.Remove(comentario);
            _context.SaveChanges();

            return Ok(new { mensaje = "Comentario eliminado correctamente" });
        }
    }

}