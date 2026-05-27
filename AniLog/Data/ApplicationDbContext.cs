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

        
        public DbSet<AnimeItem> Animes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}