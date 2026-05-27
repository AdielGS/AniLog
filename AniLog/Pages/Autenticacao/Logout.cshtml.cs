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
        public async Task<IActionResult> OnGetAsync()
        {
            return await EfetuarLogoutAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return await EfetuarLogoutAsync();
        }

        private async Task<IActionResult> EfetuarLogoutAsync()
        {
            // Apaga o cookie de autenticação do navegador de forma definitiva
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redireciona o usuário para a tela de Login limpa
            return RedirectToPage("/Autenticacao/Login");
        }
    }
}