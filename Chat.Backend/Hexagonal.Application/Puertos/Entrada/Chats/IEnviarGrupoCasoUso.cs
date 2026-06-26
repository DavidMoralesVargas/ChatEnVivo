namespace Hexagonal.Application.Puertos.Entrada.Chats
{
    public interface IEnviarGrupoCasoUso
    {
        Task EnviarGrupo(string mensaje, string grupo, string evento, string usuario);
    }
}
