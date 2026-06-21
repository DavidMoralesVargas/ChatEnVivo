using Hexagonal.Application.Persistencia;
using Hexagonal.Application.Puertos.Entrada.Usuarios;
using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.CasosDeUso.Usuarios
{
    public class IngresarUsuarioCasoUso : IIngresarUsuarioCasoUso
    {
        private IUsuariosRepository _usuariosRepository;

        public IngresarUsuarioCasoUso(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        public async Task<Usuario> Ingresar(string nombre, string contrasena)
        {
            var usuario = new Usuario(nombre, contrasena);

            try
            {
                return await _usuariosRepository.Ingresar(usuario);
            }
            catch (Exception)
            {
                return null!;
            }
        }
    }
}
