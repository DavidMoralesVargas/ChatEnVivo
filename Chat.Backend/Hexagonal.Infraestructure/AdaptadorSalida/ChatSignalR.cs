using Hexagonal.Application.Puertos.Salida;
using Hexagonal.Infraestructure.WebSocket;
using Microsoft.AspNetCore.SignalR;

namespace Hexagonal.Infraestructure.AdaptadorSalida
{
    public class ChatSignalR : IChatsSignalR
    {
        private readonly IHubContext<ChatHub> _hub;

        public ChatSignalR(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        public async Task EnviarMensajeAGrupo(string mensaje, string grupo, string evento)
        {
            await _hub.Clients.Group(grupo).SendAsync(evento, mensaje);
        }

        public async Task EnviarMensajeATodos(string mensaje, string evento, string IdEmisor, string usuarioId)
        {
            await _hub.Clients.AllExcept(IdEmisor).SendAsync(evento, new { Mensaje = mensaje, Usuario = usuarioId });
        }

        public async Task EnviarMensajeAUsuario(string mensaje, string usuario, string evento)
        {
            await _hub.Clients.User(usuario).SendAsync(evento, mensaje);
        }
    }
}
