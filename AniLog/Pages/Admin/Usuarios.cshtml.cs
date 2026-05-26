using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AniLog.Data;
using AniLog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AniLog.Pages.Admin
{
    [Authorize(Roles = "Admin")] // Apenas contas do tipo Admin entram aqui
    public class UsuariosModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public UsuariosModel(ApplicationDbContext context) => _context = context;

        public List<Usuario> ListaUsuarios { get; set; }

        public async Task OnGetAsync()
        {
            ListaUsuarios = await _context.Usuarios.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeletarUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                // Remove os animes do utilizador primeiro (Remoção manual em cascata)
                var animesUsuario = _context.Animes.Where(a => a.UsuarioId == id);
                _context.Animes.RemoveRange(animesUsuario);

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}