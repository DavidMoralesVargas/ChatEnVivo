using Hexagonal.Application.Persistencia;
using Hexagonal.Application.Puertos.Entrada.Chats;
using Hexagonal.Application.Puertos.Salida;
using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.CasosDeUso.Chats
{
    public class CrearCodigoCasoUso : ICrearCodigoCasoUso
    {
        private IChatsRepository _repository;
        private IUsuariosRepository _usuarioRepository;
        private IChatsSignalR _chatSignalR;

        public CrearCodigoCasoUso(IChatsRepository repository, IUsuariosRepository usuarioRepository, IChatsSignalR chatSignalR)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
            _chatSignalR = chatSignalR;
        }

        public async Task<bool> CrearCodigo(string nombre_usuario)
        {
            try
            {
                var usuario = await _usuarioRepository.BuscarUsuarioPorNombre(nombre_usuario);

                if (usuario == null)
                    return false;

                var codigo = new CodigoChat(false, usuario.Id);

                await _repository.CrearCodigo(codigo);
                await _chatSignalR.AvisarCreacion();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
