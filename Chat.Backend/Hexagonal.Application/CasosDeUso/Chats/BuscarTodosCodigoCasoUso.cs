using Hexagonal.Application.Puertos.Entrada.Chats;
using Hexagonal.Application.Puertos.Salida;
using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.CasosDeUso.Chats
{
    public class BuscarTodosCodigoCasoUso : IBuscarTodosCodigoCasoUso
    {

        private IChatsRepository _repository;

        public BuscarTodosCodigoCasoUso(IChatsRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CodigoChat>> BuscarTodosCodigos()
        {
            try
            {
                return await _repository.BuscarTodosCodigo();
            }
            catch (Exception)
            {
                return null!;
            }
        }
    }
}
