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

        // Obtener todas las publicaciones (sin restricciones)
        [HttpGet]
        public IActionResult GetPublicaciones()
        {
            // Obtén el ID del usuario autenticado desde el token JWT
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            // Filtra las publicaciones por el ID del usuario autenticado
            var publicaciones = _context.Publicaciones
                .Include(p => p.Usuario)
                .Where(p => p.UsuarioId == usuarioId) // Filtra las publicaciones del usuario autenticado
                .Select(p => new
                {
                    p.Id,
                    p.Titulo,
                    p.Contenido,
                    Usuario = p.Usuario.Correo // Devuelve el correo del usuario asociado
                })
                .ToList();

            return Ok(publicaciones);
        }


        // Crear una nueva publicación
        [HttpPost]
        public IActionResult CreatePublicacion([FromBody] Publicacion model)
        {
            // Obtén el ID del usuario autenticado desde el token JWT
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            // Asigna el ID del usuario autenticado a la publicación
            model.UsuarioId = usuarioId;

            _context.Publicaciones.Add(model);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPublicaciones), new { id = model.Id }, model);
        }

        // Editar una publicación (solo el propietario puede editarla)
        [HttpPut("{id}")]
        public IActionResult EditPublicacion(int id, [FromBody] Publicacion model)
        {
            // Obtén el ID del usuario autenticado desde el token JWT
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            // Busca la publicación en la base de datos
            var publicacion = _context.Publicaciones.SingleOrDefault(p => p.Id == id);

            if (publicacion == null)
            {
                return NotFound(new { mensaje = "Publicación no encontrada" });
            }

            // Verifica si el usuario autenticado es el propietario
            if (publicacion.UsuarioId != usuarioId)
            {
                return Unauthorized(new { mensaje = "No tienes permiso para editar esta publicación" });
            }

            // Actualiza los datos de la publicación
            publicacion.Titulo = model.Titulo;
            publicacion.Contenido = model.Contenido;

            _context.SaveChanges();

            return Ok(new { mensaje = "Publicación actualizada correctamente" });
        }

        // Eliminar una publicación (solo el propietario puede eliminarla)
        [HttpDelete("{id}")]
        public IActionResult DeletePublicacion(int id)
        {
            // Obtén el ID del usuario autenticado desde el token JWT
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            // Busca la publicación en la base de datos
            var publicacion = _context.Publicaciones.SingleOrDefault(p => p.Id == id);

            if (publicacion == null)
            {
                return NotFound(new { mensaje = "Publicación no encontrada" });
            }

            // Verifica si el usuario autenticado es el propietario
            if (publicacion.UsuarioId != usuarioId)
            {
                return Unauthorized(new { mensaje = "No tienes permiso para eliminar esta publicación" });
            }

            // Elimina la publicación
            _context.Publicaciones.Remove(publicacion);
            _context.SaveChanges();

            return Ok(new { mensaje = "Publicación eliminada correctamente" });
        }
    }
}

