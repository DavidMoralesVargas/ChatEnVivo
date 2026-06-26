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

        public async Task<List<Usuario>> BuscarTodos()
        {
            using var connection = new NpgsqlConnection(_connectionString);

            string query = "SELECT * FROM usuarios";

            var usuario = await connection.QueryAsync<Usuario>(query);

            return usuario.ToList();
        }

        public async Task<Usuario> BuscarUsuario(string email)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            string query = "SELECT * FROM usuarios WHERE email = @email";

            var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(query, new { email = email});

            return usuario!;
        }

        public async Task<Usuario> BuscarUsuarioPorNombre(string nombre)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            string query = "SELECT * FROM usuarios WHERE nombre_usuario = @nombreUsu";

            var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(query, new { nombreUsu = nombre });

            return usuario!;
        }

        public async Task<Usuario> Ingresar(Usuario usuario)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            string query = "SELECT * FROM usuarios WHERE nombre_usuario = @nombre_usuario AND contrasena = @contrasena";

            var usuarioIngresado = await connection.QueryFirstOrDefaultAsync<Usuario>(query,
                new
                {
                    nombre_usuario = usuario.Nombre_Usuario,
                    contrasena = usuario.Contrasena
                });

            return usuarioIngresado!;
        }

        public async Task<bool> RegistrarUsuario(Usuario usuario)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            string query = "INSERT INTO \"usuarios\"(nombre_usuario, email, contrasena) VALUES (@nombre_usuario, @email, @contrasena)";

            var filasAfectadas = await connection.ExecuteAsync(query,
                new
                {
                    nombre_usuario = usuario.Nombre_Usuario,
                    email = usuario.Email,
                    contrasena = usuario.Contrasena
                });

            return filasAfectadas != 0;
        }

    }
}
