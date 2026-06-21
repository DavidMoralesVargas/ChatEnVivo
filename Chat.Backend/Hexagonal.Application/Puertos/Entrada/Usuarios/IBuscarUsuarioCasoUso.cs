using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.Puertos.Entrada.Usuarios
{
    public interface IBuscarUsuarioCasoUso
    {
        Task<Usuario> BuscarUsuario(string Email);
    }
}
