namespace Hexagonal.Domain.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nombre_Usuario { get; set; }
        public string? Email { get; set; }
        public string? Contrasena { get; set; }

        public Usuario(string Nombre, string Email, string contrasena)
        {
            this.Nombre_Usuario = Nombre;
            this.Email = Email;
            this.Contrasena = contrasena;
        }
    }
}
