using Microsoft.EntityFrameworkCore;
using WebQuizApp.Models;

namespace WebQuizApp.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) 
        { 

        }

        public DbSet<Game> Games { get; set; }
    }
}
