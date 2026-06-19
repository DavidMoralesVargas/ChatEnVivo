
using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.Persistencia
{
    public interface IUsuariosRepository
    {
        Task<dynamic> BuscarUsuario(string email); //cambio
        bool VerificarContrasena(string password);
        Usuario RegistrarUsuario(string usuario, string email, string contrasena);
    }
}
