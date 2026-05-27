using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AniLog.Data;
using AniLog.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AniLog.Pages
{
    [Authorize] // Exige estar logado
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<AnimeItem> Assistidos { get; set; }
        public List<AnimeItem> QueroAssistir { get; set; }

        public async Task OnGetAsync()
        {
            // Pega o ID do utilizador logado no cookie da sessão
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int loggedInUserId))
            {
                var todosAnimes = await _context.Animes
                    .Where(a => a.UsuarioId == loggedInUserId) // Filtro essencial
                    .ToListAsync();

                Assistidos = todosAnimes.Where(a => a.Status == "Assistido").ToList();
                QueroAssistir = todosAnimes.Where(a => a.Status == "Quero Assistir").ToList();
            }
        }

        // NOVO MÉTODO: Executa quando o formulário de exclusão é enviado
        public async Task<IActionResult> OnPostDeletarAsync(int id)
        {
            // Procura o anime pelo ID no banco de dados
            var anime = await _context.Animes.FindAsync(id);

            if (anime != null)
            {
                // Remove do banco e salva as alterações
                _context.Animes.Remove(anime);
                await _context.SaveChangesAsync();
            }

            // Recarrega a página atual atualizada
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostAtualizarEpisodioAsync(int animeId, int epAtual)
        {
            var anime = await _context.Animes.FindAsync(animeId);
            if (anime != null)
            {
                // Atualiza apenas o progresso do episódio onde o usuário parou
                anime.EpisodioAtual = epAtual;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}