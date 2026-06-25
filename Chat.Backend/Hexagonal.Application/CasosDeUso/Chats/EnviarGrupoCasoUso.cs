using Hexagonal.Application.Puertos.Entrada.Chats;
using Hexagonal.Application.Puertos.Salida;

namespace Hexagonal.Application.CasosDeUso.Chats
{
    public class EnviarGrupoCasoUso : IEnviarGrupoCasoUso
    {
        private readonly IChatsSignalR _chat;

        public EnviarGrupoCasoUso(IChatsSignalR chat) 
        {
            _chat = chat;
        }

        public async Task EnviarGrupo(string mensaje, string grupo, string evento)
        {
            try
            {
                await _chat.EnviarMensajeAGrupo(mensaje, grupo, evento);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
