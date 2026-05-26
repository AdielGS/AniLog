using AniLog.Models;
using Microsoft.EntityFrameworkCore;

namespace AniLog.Data
{
    // A classe herda de DbContext, que vem do Entity Framework
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Aqui nós dizemos que o Model AnimeItem vai virar uma tabela no banco chamada "Animes"
        public DbSet<AnimeItem> Animes { get; set; }
    }
}