using Hexagonal.Application.Persistencia;
using Hexagonal.Domain.Entidades;

namespace Hexagonal.Infraestructure.Persistencia
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly string _connectionString;

        
        public UsuariosRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool BuscarUsuario(string email)
        {
            throw new NotImplementedException();
        }

        public Usuario RegistrarUsuario(string usuario, string email, string contrasena)
        {
            throw new NotImplementedException();
        }

        public bool VerificarContrasena(string password)
        {
            throw new NotImplementedException();
        }
    }
}
