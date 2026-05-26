using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AniLog.Pages.Autenticacao
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class LogoutModel : PageModel
    {
        // Caso o utilizador clique no link direto (Método GET)
        public async Task<IActionResult> OnGetAsync()
        {
            return await ExecutarLogoutAsync();
        }

        // Caso o formulário envie uma requisição (Método POST)
        public async Task<IActionResult> OnPostAsync()
        {
            return await ExecutarLogoutAsync();
        }

        private async Task<IActionResult> ExecutarLogoutAsync()
        {
            // Destrói o Cookie de Autenticação gravado no navegador do utilizador
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redireciona o utilizador de volta para a página de Login limpa
            return RedirectToPage("/Autenticacao/Login");
        }
    }
}