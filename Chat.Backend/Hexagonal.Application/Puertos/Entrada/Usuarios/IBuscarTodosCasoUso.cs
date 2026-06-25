using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.Puertos.Entrada.Usuarios
{
    public interface IBuscarTodosCasoUso
    {
        Task<List<Usuario>> BuscarTodos();
    }
}
