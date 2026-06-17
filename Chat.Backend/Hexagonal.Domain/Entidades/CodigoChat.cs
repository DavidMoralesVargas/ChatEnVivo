namespace Hexagonal.Domain.Entidades
{
    public class CodigoChat
    {
        public int Id { get; set; }
        public int Codigo {  get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }     
        public int UsuarioCreacionId { get; set; }

        public CodigoChat(int codigo, bool Activo, int UsuarioId)
        {
            this.Codigo = codigo;
            this.FechaCreacion = DateTime.Now;
            this.UsuarioCreacionId = UsuarioId;
        }

    }
}
