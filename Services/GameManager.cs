using WebQuizApp.Models;

namespace WebQuizApp.Services
{
    public static class GameManager
    {
        public static Game CurrentGame { get; private set; } = new Game();

        public static void StartGame(List<Question> questions)
        {
            CurrentGame = new Game
            {
                Questions = questions,
                IsActive = true,
                CurrentQuestionIndex = 0
            };
        }

        public static void EndGame()
        {
            CurrentGame.IsActive = false;
            CurrentGame.Players.Clear();
        }

        public static void ResetGame()
        {
            CurrentGame = new Game();
        }

        public static void ResetScores()
        {
            CurrentGame = new Game();
        }
    }
}
