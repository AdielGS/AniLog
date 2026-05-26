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