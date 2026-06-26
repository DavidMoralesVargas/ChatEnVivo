namespace Hexagonal.Domain.Entidades
{
    public class CodigoChat
    {
        public int Id { get; set; }
        public int codigoChat {  get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }     
        public int UsuarioCreacionId { get; set; }


        public CodigoChat() { }

        public CodigoChat(bool Activo, int UsuarioId)
        {
            this.codigoChat = UsuarioId;
            this.FechaCreacion = DateTime.Now;
            this.UsuarioCreacionId = UsuarioId;
            this.Activo = Activo;
        }

    }
}
