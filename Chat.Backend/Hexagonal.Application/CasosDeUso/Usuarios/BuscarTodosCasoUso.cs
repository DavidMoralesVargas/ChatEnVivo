using Hexagonal.Application.Persistencia;
using Hexagonal.Application.Puertos.Entrada.Usuarios;
using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.CasosDeUso.Usuarios
{
    public class BuscarTodosCasoUso : IBuscarTodosCasoUso
    {
        private IUsuariosRepository _repository;

        public BuscarTodosCasoUso(IUsuariosRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Usuario>> BuscarTodos()
        {
            try
            {
                return await _repository.BuscarTodos();
            }
            catch(Exception)
            {
                return null!;
            }
        }
    }
}
