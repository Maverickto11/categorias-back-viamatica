using categorias_back_viamatica.Data;
using categorias_back_viamatica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace categorias_back_viamatica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
       
        
            private readonly ApplicationDbContext _context;
            private readonly IConfiguration _configuration;

            public AuthController(ApplicationDbContext context, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            // Endpoint para login
            [HttpPost("login")]
            public IActionResult Login([FromBody] LoginModel model)
            {
                var usuario = _context.Usuarios
                                      .SingleOrDefault(u => u.Correo == model.Correo && u.Contraseña == model.Contraseña);

                if (usuario == null)
                {
                    return Unauthorized();  // Retorna un error si las credenciales son incorrectas
                }

                var token = GenerarToken(usuario);
                return Ok(new { Token = token });
            }
        [HttpPost("registro")]
        public IActionResult RegistrarUsuario([FromBody] Usuario model)
        {
            if (string.IsNullOrWhiteSpace(model.Nombre) ||
                string.IsNullOrWhiteSpace(model.Correo) ||
                string.IsNullOrWhiteSpace(model.Contraseña))
            {
                return BadRequest("Todos los campos son obligatorios.");
            }

            // Verificar si el correo ya está registrado
            if (_context.Usuarios.Any(u => u.Correo == model.Correo))
            {
                return Conflict("El correo ya está registrado.");
            }

            // Encriptar la contraseña (usa un método de encriptación seguro, como BCrypt)
            model.Contraseña = BCrypt.Net.BCrypt.HashPassword(model.Contraseña);

            // Guardar el usuario en la base de datos
            _context.Usuarios.Add(model);
            _context.SaveChanges();

            return Ok(new { message = "Usuario registrado exitosamente." });
        }





        // Método para generar el JWT
        private string GenerarToken(Usuario usuario)
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, usuario.Correo),
                new Claim("UsuarioId", usuario.Id.ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddHours(1),  // Expiración del token
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);  // Genera y retorna el token
            }
        }
        public class LoginModel
        {
            public string Correo { get; set; }
            public string Contraseña { get; set; }
        }

    
}
