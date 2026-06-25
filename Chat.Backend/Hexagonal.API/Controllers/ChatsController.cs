using Hexagonal.Application.Puertos.Entrada.Chats;
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

        public ChatsController(IEnviarGrupoCasoUso grupoCasoUso,
                               IEnviarTodosCasoUso todosCasoUso,
                               IEnviarUsuarioCasoUso usuarioCasoUso) 
        { 
            _grupoCasoUso = grupoCasoUso;
            _todosCasoUso = todosCasoUso;
            _usuarioCasoUso = usuarioCasoUso;
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
            await _grupoCasoUso.EnviarGrupo(mensajeDTO.Mensaje!, mensajeDTO.Grupo!, "ChatGrupal");
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
