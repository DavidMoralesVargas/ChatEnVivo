using Hexagonal.Application.Puertos.Entrada.Chats;
using Hexagonal.Application.Puertos.Salida;

namespace Hexagonal.Application.CasosDeUso.Chats
{
    public class EnviarUsuarioCasoUso : IEnviarUsuarioCasoUso
    {
        private readonly IChatsSignalR _chat;

        public EnviarUsuarioCasoUso(IChatsSignalR chat)
        {
            _chat = chat;
        }

        public async Task EnviarUsuario(string mensaje, string usuario, string evento)
        {
            try
            {
                await _chat.EnviarMensajeAUsuario(mensaje, usuario, evento);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
