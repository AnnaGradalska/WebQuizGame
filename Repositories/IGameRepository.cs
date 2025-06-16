using WebQuizApp.Models;

namespace WebQuizApp.Repositories
{
    public interface IGameRepository
    {
        Task<Game?> GetByCodeAsync(string code);
        Task AddAsync(Game game);
        Task SaveChangesAsync();
    }
}
