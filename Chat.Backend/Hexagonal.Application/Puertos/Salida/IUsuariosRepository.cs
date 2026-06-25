
using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.Persistencia
{
    public interface IUsuariosRepository
    {
        Task<Usuario> BuscarUsuario(string email); 
        Task<Usuario> Ingresar(Usuario usuario);
        Task<bool> RegistrarUsuario(Usuario usuario);
        Task<List<Usuario>> BuscarTodos();
    }
}
