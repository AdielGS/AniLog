using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AniLog.Data;
using AniLog.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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
            _context.Attach(Anime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Animes.Any(e => e.Id == Anime.Id)) return NotFound();
                else throw;
            }

            return RedirectToPage("/Index");
        }
    }
}