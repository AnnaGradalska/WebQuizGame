using WebQuizApp.Models;
using WebQuizApp.Repositories;

namespace WebQuizApp.Services
{
    public class GameService
    {
        private readonly IGameRepository _repository;

        public GameService(IGameRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> StartGameAsync(List<Question> questions)
        {
            var game = new Game
            {
                Code = Guid.NewGuid().ToString("N")[..6].ToUpper(),
                Questions = questions,
                IsActive = true,
                CurrentQuestionIndex = 0
            };

            await _repository.AddAsync(game);
            await _repository.SaveChangesAsync();

            return game.Code;
        }

        public async Task EndGameAsync(string code)
        {
            var game = await _repository.GetByCodeAsync(code);
            if (game is null)
            {
                return;
            }

            game.IsActive = false;
            await _repository.SaveChangesAsync();
        }


        public async Task ResetScoresAsync(string code)
        {
            var game = await _repository.GetByCodeAsync(code);
            if (game is null)
            {
                return;
            }

            foreach (var player in game.Players)
            {
                player.Score = 0;
                player.HasAnswered = false;
            }

            await _repository.SaveChangesAsync();

        }

        public async Task<Game?> GetGameByCodeAsync(string code)
        {
            return await _repository.GetByCodeAsync(code);
        }

        public async Task SaveChangesAsync()
        {
            await _repository.SaveChangesAsync();
        }
    }
}
