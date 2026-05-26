using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AniLog.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AniLog.Pages.Autenticacao
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public LoginModel(ApplicationDbContext context) => _context = context;

        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Senha { get; set; }
        public string MensagemErro { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Senha))
            {
                MensagemErro = "Por favor, preencha todos os campos.";
                return Page();
            }

            // Gera o hash exatamente igual ao que foi salvo no Registrar
            var hash = GerarHash(Senha);

            // Busca o usuário ignorando espaços e maiúsculas
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.Trim().ToLower() == Email.Trim().ToLower() && u.SenhaHash == hash);

            if (usuario == null)
            {
                MensagemErro = "E-mail ou senha incorretos.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Propriedades do Cookie para evitar que ele seja bloqueado em localhost (ambiente de desenvolvimento)
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            // Força o redirecionamento para a raiz absoluta do site
            return RedirectToPage("/Index");
        }

        private string GerarHash(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(senha));

            StringBuilder builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2")); // Força minúsculas idêntico ao cadastro
            }
            return builder.ToString();
        }
    }
}