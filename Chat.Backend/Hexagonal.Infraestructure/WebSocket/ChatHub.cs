using Microsoft.AspNetCore.SignalR;

namespace Hexagonal.Infraestructure.WebSocket
{
    public class ChatHub : Hub
    {
        public async Task UnirsePartida(string partidaId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                partidaId
            );
        }

        public async Task SalirPartida(string partidaId)
        {
            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                partidaId
            );
        }

    }
}
