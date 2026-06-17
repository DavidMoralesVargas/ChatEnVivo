namespace Hexagonal.Application.Puertos.Entrada.Usuarios
{
    public interface IRegistrarUsuarioCasoUso
    {
        Task<string> Registrar(string NomUsuario, string Email, string Contrasena);
    }
}
