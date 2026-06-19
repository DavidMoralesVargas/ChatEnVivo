using Microsoft.Extensions.Configuration;

namespace Hexagonal.Infraestructure.Persistencia
{
    public class UsuariosRepository 
    {
        private readonly string _connectionString;

        
        public UsuariosRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
