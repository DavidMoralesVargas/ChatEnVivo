namespace Hexagonal.Application.Puertos.Salida
{
    public interface IChatsSignalR
    {
        Task EnviarMensajeATodos(string mensaje, string evento, string IdEmisor, string usuarioId);
        Task EnviarMensajeAUsuario(string mensaje, string usuario, string evento);
        Task EnviarMensajeAGrupo(string mensaje, string grupo, string evento);
    }
}
