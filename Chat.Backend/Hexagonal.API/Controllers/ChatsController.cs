using Hexagonal.Application.Puertos.Entrada.Chats;
using Hexagonal.Domain.Entidades;
using Hexagonal.Infraestructure.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hexagonal.API.Controllers
{
    [ApiController]
    [Route("/api/chat")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatsController : ControllerBase
    {
        private IEnviarGrupoCasoUso _grupoCasoUso;
        private IEnviarTodosCasoUso _todosCasoUso;
        private IEnviarUsuarioCasoUso _usuarioCasoUso;
        private IBuscarTodosCodigoCasoUso _todosCodigosCasoUso;
        private ICrearCodigoCasoUso _crearCasoUso;

        public ChatsController(IEnviarGrupoCasoUso grupoCasoUso,
                               IEnviarTodosCasoUso todosCasoUso,
                               IEnviarUsuarioCasoUso usuarioCasoUso,
                               IBuscarTodosCodigoCasoUso todosCodigosCasoUso,
                               ICrearCodigoCasoUso crearCodigo) 
        { 
            _grupoCasoUso = grupoCasoUso;
            _todosCasoUso = todosCasoUso;
            _usuarioCasoUso = usuarioCasoUso;
            _todosCodigosCasoUso = todosCodigosCasoUso;
            _crearCasoUso = crearCodigo;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            return Ok(await _todosCodigosCasoUso.BuscarTodosCodigos());
        }

        [HttpPost("crearCodigo")]
        public async Task<IActionResult> CrearCodigo()
        {
            var nombre_usuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var respuesta = await _crearCasoUso.CrearCodigo(nombre_usuario);
            if (respuesta)
            {
                return Ok();
            }
            return BadRequest();
        }


        [HttpPost("todos")]
        public async Task<IActionResult> EnviarTodos([FromBody] MensajeDTO mensajeDTO)
        {
            string usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            await _todosCasoUso.EnviarTodos(mensajeDTO.Mensaje!, "ChatGlobal", mensajeDTO.IdEmisor!, usuarioId);
            return Ok();
        }

        [HttpPost("grupo")]
        public async Task<IActionResult> EnviarGrupo([FromBody] MensajeDTO mensajeDTO)
        {
            string usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            await _grupoCasoUso.EnviarGrupo(mensajeDTO.Mensaje!, mensajeDTO.Grupo!, "ChatGrupal", usuarioId);
            return Ok();
        }

        [HttpPost("usuario")]
        public async Task<IActionResult> EnviarUsuario([FromBody] MensajeDTO mensajeDTO)
        {
            await _usuarioCasoUso.EnviarUsuario(mensajeDTO.Mensaje!, mensajeDTO.Usuario!, "ChatIndivudual");
            return Ok();
        }
    }
}
