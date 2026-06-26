using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.Puertos.Salida
{
    public interface IChatsRepository
    {
        Task CrearCodigo(CodigoChat codigo);
        Task<List<CodigoChat>> BuscarTodosCodigo();
    }
}
