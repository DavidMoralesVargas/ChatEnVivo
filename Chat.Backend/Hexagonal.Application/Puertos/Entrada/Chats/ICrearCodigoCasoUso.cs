using Hexagonal.Domain.Entidades;

namespace Hexagonal.Application.Puertos.Entrada.Chats
{
    public interface ICrearCodigoCasoUso
    {
        Task<bool> CrearCodigo(string nombre_usuario);
    }
}
