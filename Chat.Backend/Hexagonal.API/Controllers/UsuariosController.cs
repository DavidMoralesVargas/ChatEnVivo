using Hexagonal.Application.Puertos.Entrada.Usuarios;
using Hexagonal.Domain.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hexagonal.API.Controllers
{
    [ApiController]
    [Route("/api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private IBuscarUsuarioCasoUso _buscarCasoUso;
        private IIngresarUsuarioCasoUso _ingresarCasoUso;
        private IRegistrarUsuarioCasoUso _registrarCasoUso;
        private IConfiguration _configuration;

        public UsuariosController(IBuscarUsuarioCasoUso buscarCasoUso,
                                  IIngresarUsuarioCasoUso ingresarCasoUso,
                                  IRegistrarUsuarioCasoUso registrarCasoUso,
                                  IConfiguration configuration)
        {
            _buscarCasoUso = buscarCasoUso;
            _ingresarCasoUso = ingresarCasoUso;
            _registrarCasoUso = registrarCasoUso;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Buscar([FromQuery] string email)
        {
            var respuesta = await _buscarCasoUso.BuscarUsuario(email);
            if(respuesta != null)
            {
                return Ok(respuesta);
            }
            return BadRequest();
        }

        [HttpPost("Ingresar")]
        public async Task<IActionResult> Ingresar([FromBody] Usuario usuario)
        {
            var respuesta = await _ingresarCasoUso.Ingresar(usuario.Nombre_Usuario!, usuario.Contrasena!);
            if (respuesta != null)
            {
                return Ok(ConstruirToken(respuesta));
            }
            return BadRequest();
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] Usuario usuario)
        {
            var respuesta = await _registrarCasoUso.Registrar(usuario.Nombre_Usuario!, usuario.Email!, usuario.Contrasena!);
            if (respuesta)
            {
                return Ok(respuesta);
            }
            return BadRequest();
        }

        private string ConstruirToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, usuario.Nombre_Usuario!),
                new("email", usuario.Email!),
                new("sub", usuario.Nombre_Usuario!),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);
            var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
