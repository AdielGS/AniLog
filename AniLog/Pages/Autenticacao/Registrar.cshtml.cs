using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AniLog.Data;
using AniLog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AniLog.Pages.Autenticacao
{
    public class RegistrarModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public RegistrarModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public RegisterInput Input { get; set; }
        public string MensagemErro { get; set; }

        public class RegisterInput
        {
            [Required] public string Nome { get; set; }
            [Required][EmailAddress] public string Email { get; set; }
            [Required] public string Senha { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == Input.Email.ToLower());
            if (emailExiste)
            {
                MensagemErro = "Este e-mail já se encontra registado no sistema.";
                return Page();
            }

            // Conta o número de utilizadores. O primeiro a registar-se torna-se Admin automaticamente para fins de teste.
            string perfil = await _context.Usuarios.AnyAsync() ? "Usuario" : "Admin";

            var usuario = new Usuario
            {
                Nome = Input.Nome,
                Email = Input.Email,
                Perfil = perfil,
                SenhaHash = GerarHash(Input.Senha)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Autenticacao/Login");
        }

        private string GerarHash(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToHexString(bytes);
        }
    }
}