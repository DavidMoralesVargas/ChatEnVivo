using Hexagonal.Application.Persistencia;
using Hexagonal.Application.Puertos.Entrada.Usuarios;
using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.CasosDeUso.Usuarios
{
    public class BuscarUsuarioCasoUso : IBuscarUsuarioCasoUso
    {
        private IUsuariosRepository _usuariosRepository;

        public BuscarUsuarioCasoUso(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        public async Task<Usuario> BuscarUsuario(string Email)
        {
            try
            {
                return await _usuariosRepository.BuscarUsuario(Email!);
            }
            catch (Exception)
            {
                return null!;
            }
        }
    }
}
