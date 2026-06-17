namespace Hexagonal.Application.Puertos.Entrada.CodigosChats
{
    public interface ICrearCodigoCasoUso
    {
        Task<string> CrearCodigo(int codigo, bool activo, int usuarioid);
    }
}
