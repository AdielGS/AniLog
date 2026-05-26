using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AniLog.Data;
using AniLog.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AniLog.Pages.Animes
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AnimeItem Anime { get; set; }

        public void OnGet()
        {
            // Executado quando a página carrega pela primeira vez
        }

        // Executado quando você clica no botão "Salvar" do formulário
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // VALIDAÇÃO: Verifica se já existe um registro com o mesmo título (ignorando maiúsculas/minúsculas)
            bool jaExiste = await _context.Animes
                .AnyAsync(a => a.Titulo.ToLower() == Anime.Titulo.ToLower());

            if (jaExiste)
            {
                // Se já existe, adiciona um erro customizado na tela e impede o salvamento
                ModelState.AddModelError("Duplicado", "Você já adicionou esse anime/filme na sua lista!");
                return Page();
            }

            _context.Animes.Add(Anime);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}