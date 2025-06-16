using WebQuizApp.Models;
using WebQuizApp.Data;
using Microsoft.EntityFrameworkCore;

namespace WebQuizApp.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameDbContext _context;

        public GameRepository(GameDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Game?> GetByCodeAsync(string code)
        {
            return await _context.Games
                .Include(g => g.Players)
                .Include(g => g.Questions)
                .FirstOrDefaultAsync(g => g.Code == code);
        }

        public async Task AddAsync(Game game)
        {
            await _context.Games.AddAsync(game);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
