using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AniLog.Data;
using AniLog.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text.Json;
using System.Security.Claims; // Necessário para ler as Claims do usuário logado

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
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Captura o ID do usuário que está logado na sessão atual
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                // Se por algum motivo o cookie sumiu, manda logar novamente
                return RedirectToPage("/Autenticacao/Login");
            }

            // 2. Injeta o ID do usuário logado no objeto que vai para o banco
            Anime.UsuarioId = usuarioId;

            // Remove o Usuario do ModelState para evitar falsos erros de validação
            ModelState.Remove("Anime.Usuario");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // VALIDAÇÃO COM FILTRO DE USUÁRIO: 
            // Agora ele só barra se o MESMO usuário tentar cadastrar o mesmo título repetido
            bool jaExiste = await _context.Animes
                .AnyAsync(a => a.UsuarioId == usuarioId && a.Titulo.ToLower() == Anime.Titulo.ToLower());

            if (jaExiste)
            {
                ModelState.AddModelError("Duplicado", "Você já adicionou esse anime/filme na sua lista!");
                return Page();
            }

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "AniLogApp");
                var url = $"https://api.jikan.moe/v4/anime?q={Uri.EscapeDataString(Anime.Titulo)}&limit=1";
                var response = await client.GetStringAsync(url);

                using var doc = JsonDocument.Parse(response);
                var root = doc.RootElement;

                if (root.TryGetProperty("data", out var dataArray) && dataArray.GetArrayLength() > 0)
                {
                    var animeData = dataArray[0];

                    // 1. Captura a Sinopse (se houver)
                    if (animeData.TryGetProperty("synopsis", out var synopsisProp))
                    {
                        Anime.Sinopse = synopsisProp.GetString();
                    }

                    // 2. Captura o total de episódios (se houver e não for nulo na API)
                    if (animeData.TryGetProperty("episodes", out var epsProp) && epsProp.ValueKind != JsonValueKind.Null)
                    {
                        Anime.TotalEpisodios = epsProp.GetInt32();
                    }
                }
            }
            catch
            {
                // Se a API falhar, define valores padrão para não quebrar o fluxo
                Anime.Sinopse = "Sinopse não disponível no momento.";
                Anime.TotalEpisodios = 0;
            }

            _context.Animes.Add(Anime);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}