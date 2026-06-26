using Dapper;
using Hexagonal.Application.Puertos.Salida;
using Hexagonal.Domain.Entidades;
using Npgsql;

namespace Hexagonal.Infraestructure.Persistencia
{
    public class ChatsRepository : IChatsRepository
    {

        private readonly string _connectionString;

        public ChatsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<CodigoChat>> BuscarTodosCodigo()
        {
            using var connection = new NpgsqlConnection(_connectionString);

            string query = "SELECT * FROM codigochat";

            var usuario = await connection.QueryAsync<CodigoChat>(query);

            return usuario.ToList();
        }

        public async Task CrearCodigo(CodigoChat codigo)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            string query = "INSERT INTO \"codigochat\"(codigochat, fechacreacion, activo, usuariocreacionid) VALUES(@codigo, @fecha, @activo, @usuarioId)";

            var usuario = await connection.ExecuteAsync(query, new
            {
                codigo = codigo.codigoChat,
                fecha = codigo.FechaCreacion,
                activo = codigo.Activo,
                usuarioId = codigo.UsuarioCreacionId
            });

        }
    }
}
