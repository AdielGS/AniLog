using AniLog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// CONFIGURAÇÃO DO COOKIE DE LOGIN
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Autenticacao/Login"; // Página padrão de login
        options.AccessDeniedPath = "/Autenticacao/AcessoNegado"; // Página se tentar acessar algo restrito
        options.ExpireTimeSpan = TimeSpan.FromHours(2); // Duração da sessão
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

var app = builder.Build();

// --- BLOCO TEMPORÁRIO DE RESET DE SEGURANÇA ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AniLog.Data.ApplicationDbContext>();

    // 1. Limpa os utilizadores antigos para remover dados corrompidos das tabelas minúsculas
    context.Usuarios.RemoveRange(context.Usuarios);
    context.SaveChanges();

    // 2. Cria a função para gerar o hash em minúsculas idêntico ao sistema atual do Login
    using var sha = System.Security.Cryptography.SHA256.Create();
    var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes("admin"));
    var sb = new System.Text.StringBuilder();
    foreach (var b in bytes) sb.Append(b.ToString("x2"));
    string senhaGarantida = sb.ToString();

    // 3. Insere o teu utilizador Admin definitivo
    var adminUser = new AniLog.Models.Usuario
    {
        Nome = "Adiel Administrador",
        Email = "admin@anilog.com",
        Perfil = "Admin",
        SenhaHash = senhaGarantida
    };

    context.Usuarios.Add(adminUser);
    context.SaveChanges();
}
// --- FIM DO BLOCO TEMPORÁRIO ---

// Configurar o pipeline de requisições HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ADICIONAR ESTAS DUAS LINHAS EXATAMENTE NESTA ORDEM:
app.UseAuthentication(); // Quem é você?
app.UseAuthorization();  // O que você pode acessar?

app.MapRazorPages();

app.Run();