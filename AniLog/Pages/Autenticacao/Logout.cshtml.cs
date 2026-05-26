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
        // Se o usuário acessar via link direto
        public async Task<IActionResult> OnGetAsync()
        {
            return await EfetuarLogout();
        }

        // Se o usuário acessar via botão/post
        public async Task<IActionResult> OnPostAsync()
        {
            return await EfetuarLogout();
        }

        private async Task<IActionResult> EfetuarLogout()
        {
            // Limpa o cookie de autenticação do navegador de forma definitiva
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redireciona para a tela de Login limpa
            return RedirectToPage("/Autenticacao/Login");
        }
    }
}