using WebQuizApp.Models;

namespace WebQuizApp.Services
{
    public static class GameManager
    {
        public static Dictionary<string, Game> Games { get; private set; } = new();

        public static Game CreateGame(List<Question> questions)
        {
            var game = new Game
            {
                Questions = questions,
                IsActive = true,
                CurrentQuestionIndex = 0,
                Code = Guid.NewGuid().ToString("N")[..6].ToUpper()
            };

            Games[game.Code] = game;

            return game;
        }

        public static Game? GetGame(string code)
        {
            Games.TryGetValue(code.ToUpper(), out var game);
            return game;
        }

        //public static void EndGame()
        //{
        //    CurrentGame.IsActive = false;
        //    CurrentGame.Players.Clear();
        //}

        //public static void ResetGame()
        //{
        //    CurrentGame = new Game();
        //}

        //public static void ResetScores()
        //{
        //    CurrentGame = new Game();
        //}
    }
}
