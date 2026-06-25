namespace Hexagonal.Application.Puertos.Entrada.Chats
{
    public interface IEnviarUsuarioCasoUso
    {
        Task EnviarUsuario(string mensaje, string usuario, string evento);
    }
}
