namespace Hexagonal.Application.Puertos.Entrada.Usuarios
{
    public interface IIngresarUsuarioCasoUso
    {
        Task<string> Ingresar(string email, string contrasena);
    }
}
