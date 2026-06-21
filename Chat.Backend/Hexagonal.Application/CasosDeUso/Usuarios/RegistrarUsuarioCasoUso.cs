using Hexagonal.Application.Persistencia;
using Hexagonal.Application.Puertos.Entrada.Usuarios;
using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.CasosDeUso.Usuarios
{
    public class RegistrarUsuarioCasoUso : IRegistrarUsuarioCasoUso
    {
        private IUsuariosRepository _usuariosRepository;

        public RegistrarUsuarioCasoUso(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        public async Task<bool> Registrar(string NomUsuario, string Email, string Contrasena)
        {
            try
            {
                var usuario = new Usuario(NomUsuario, Email, Contrasena);

                var respuesta = await _usuariosRepository.RegistrarUsuario(usuario);

                return respuesta;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
