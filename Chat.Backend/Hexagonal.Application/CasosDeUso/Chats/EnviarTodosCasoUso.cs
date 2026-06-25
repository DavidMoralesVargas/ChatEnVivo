using Hexagonal.Application.Puertos.Entrada.Chats;
using Hexagonal.Application.Puertos.Salida;

namespace Hexagonal.Application.CasosDeUso.Chats
{
    public class EnviarTodosCasoUso : IEnviarTodosCasoUso
    {
        private readonly IChatsSignalR _chat;

        public EnviarTodosCasoUso(IChatsSignalR chat)
        {
            _chat = chat;
        }

        public async Task EnviarTodos(string mensaje, string evento, string IdEmisor, string usuarioId)
        {
            try
            {
                await _chat.EnviarMensajeATodos(mensaje, evento, IdEmisor, usuarioId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
