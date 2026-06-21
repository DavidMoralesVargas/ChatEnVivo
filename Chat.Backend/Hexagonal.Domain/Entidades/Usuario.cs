namespace Hexagonal.Domain.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nombre_Usuario { get; set; }
        public string? Email { get; set; }
        public string? Contrasena { get; set; }


        public Usuario() { }

        //Constructor para el Dapper
        public Usuario(int id, string nombre_usuario, string email, string contrasena)
        {
            Id = id;
            Nombre_Usuario = nombre_usuario;
            Email = email;
            Contrasena = contrasena;
        }


        public Usuario(string Nombre_Usuario, string contrasena)
        {
            this.Nombre_Usuario = Nombre_Usuario;
            this.Contrasena = contrasena;
        }

        public Usuario(string Nombre, string Email, string contrasena)
        {
            this.Nombre_Usuario = Nombre;
            this.Email = Email;
            this.Contrasena = contrasena;
        }
    }
}
