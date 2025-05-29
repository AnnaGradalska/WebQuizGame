using Microsoft.AspNetCore.SignalR;
using WebQuizApp.Services;

namespace WebQuizApp.Hubs
{
    public class GameHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public async Task StartNextQuestion()
        {
            var game = GameManager.CurrentGame;

            if (!game.IsActive || game.CurrentQuestionIndex >= game.Questions.Count)
            {
                await Clients.All.SendAsync("GameEnded");
                return;
            }

            var question = game.Questions[game.CurrentQuestionIndex];

            foreach (var player in game.Players)
            {
                player.HasAnswered = false;
            }

            await Clients.All.SendAsync("ReceiveQuestion", new
            {
                question.Id,
                question.Text,
                question.Options,
                question.TimeLimit
            });
        }

        public async Task ShowRanking()
        {
            var ranking = GameManager.CurrentGame.Players
                .OrderByDescending(p => p.Score)
                .Select(p => new { p.Username, p.Score });

            await Clients.All.SendAsync("ReceiveRanking", ranking);
        }

        public async Task Next()
        {
            GameManager.CurrentGame.CurrentQuestionIndex++;
            await StartNextQuestion();
        }

    }
}
