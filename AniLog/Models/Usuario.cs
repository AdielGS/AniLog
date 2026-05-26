using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniLog.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        public string SenhaHash { get; set; }

        [Required]
        [StringLength(20)]
        public string Perfil { get; set; } // "Admin" ou "Usuario"
    }
}