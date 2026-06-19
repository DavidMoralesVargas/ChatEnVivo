using Dapper;
using Hexagonal.Application.Persistencia;
using Hexagonal.Domain.Entidades;
using Npgsql;

namespace Hexagonal.Infraestructure.Persistencia
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly string _connectionString;

        
        public UsuariosRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<dynamic> BuscarUsuario(string email)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            string query = "SELECT * FROM usuarios";

            var usuarios = await connection.QueryAsync<dynamic>(query);

            return usuarios;
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
