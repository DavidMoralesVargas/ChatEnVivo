namespace Hexagonal.Application.Puertos.Entrada.Chats
{
    public interface IEnviarTodosCasoUso
    {
        Task EnviarTodos(string mensaje, string evento, string IdEmisor, string usuarioId);
    }
}
