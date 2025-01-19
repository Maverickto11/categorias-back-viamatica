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
