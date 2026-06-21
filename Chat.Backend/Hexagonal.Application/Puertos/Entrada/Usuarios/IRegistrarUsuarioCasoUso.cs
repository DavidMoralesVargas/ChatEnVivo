namespace Hexagonal.Application.Puertos.Entrada.Usuarios
{
    public interface IRegistrarUsuarioCasoUso
    {
        Task<bool> Registrar(string NomUsuario, string Email, string Contrasena);
    }
}
