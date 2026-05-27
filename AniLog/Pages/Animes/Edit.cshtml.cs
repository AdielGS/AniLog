using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AniLog.Data;
using AniLog.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AniLog.Pages.Animes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AnimeItem Anime { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Anime = await _context.Animes.FindAsync(id);

            if (Anime == null) return RedirectToPage("/Index");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Recupera o ID do usuário que está logado na sessão atual de cookies
            var usuarioIdLogado = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioIdLogado) || !int.TryParse(usuarioIdLogado, out int usuarioId))
            {
                return RedirectToPage("/Autenticacao/Login");
            }

            // 2. Garante por segurança que o UsuarioId do anime que está sendo editado é o mesmo de quem está logado
            Anime.UsuarioId = usuarioId;

            // Remove referências de objetos virtuais para evitar conflitos no Entity Framework
            ModelState.Remove("Anime.Usuario");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Informa ao Entity Framework para anexar e atualizar o registro modificado no PostgreSQL
            _context.Attach(Anime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Animes.Any(e => e.Id == Anime.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Index");
        }
    }
}