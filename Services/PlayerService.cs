using WebQuizApp.Repositories;
using WebQuizApp.Models;

namespace WebQuizApp.Services
{
    public class PlayerService
    {
        private readonly IGameRepository _repository;

        public PlayerService(IGameRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> JoinGameAsync(string gameCode, string username)
        {
            var game = await _repository.GetByCodeAsync(gameCode);
            if (game == null || !game.IsActive)
            {
                return false;
            }

            if (game.Players.Any(p => p.Username == username))
            {
                return false;
            }

            game.Players.Add(new Player { Username = username });
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<(bool success, bool isCorrect, int score)> SubmitAnswerAsync(string gameCode, Answer answer)
        {
            var game = await _repository.GetByCodeAsync(gameCode);
            if (game == null)
            {
                return (false, false, 0);
            }

            var player = game.Players.FirstOrDefault(p => p.Username == answer.Username);
            if (player == null || player.HasAnswered)
                return (false, false, 0);

            var question = game.Questions.ElementAtOrDefault(game.CurrentQuestionIndex);
            if (question == null)
                return (false, false, 0);

            player.HasAnswered = true;

            bool isCorrest = answer.SelectedOption == question.CorrectAnswer;
            if (isCorrest)
            {
                player.Score += 100;
            }

            await _repository.SaveChangesAsync();

            return (true, isCorrest, player.Score);
        }
    }
}
