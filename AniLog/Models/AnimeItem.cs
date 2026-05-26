using System.ComponentModel.DataAnnotations.Schema;

namespace AniLog.Models
{
    [Table("animes")]
    public class AnimeItem
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string ImagemUrl { get; set; }
        public string Status { get; set; }
        public int Nota { get; set; }

        // Vínculo com o usuário que cadastrou
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
    }
}