using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.Puertos.Entrada.Chats
{
    public interface IBuscarTodosCodigoCasoUso
    {
        Task<List<CodigoChat>> BuscarTodosCodigos();
    }
}
