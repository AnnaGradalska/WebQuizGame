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

        public async Task JoinGroup(string code)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, code);
            Console.WriteLine($"Client {Context.ConnectionId} joined group {code}");
        }

        public async Task StartNextQuestion(string code)
        {
            var game = GameManager.GetGame(code);

            if (game == null)
            {
                await Clients.Caller.SendAsync("Error", "Gra nie istnieje.");
                return;
            }

            if (!game.IsActive || game.CurrentQuestionIndex >= game.Questions.Count)
            {
                await Clients.Group(code).SendAsync("GameEnded");
                return;
            }

            var question = game.Questions[game.CurrentQuestionIndex];

            foreach (var player in game.Players)
            {
                player.HasAnswered = false;
            }

            await Clients.Group(code).SendAsync("ReceiveQuestion", new
            {
                question.Id,
                question.Text,
                question.Options,
                question.TimeLimit
            });
        }

        public async Task ShowRanking(string code)
        {
            var game = GameManager.GetGame(code);
            if (game == null)
            {
                await Clients.Caller.SendAsync("Error", "Gra nie istnieje.");
                return;
            }

            var ranking = game.Players
                .OrderByDescending(p => p.Score)
                .Select(p => new { p.Username, p.Score });

            await Clients.Group(code).SendAsync("ReceiveRanking", ranking);
        }

        public async Task Next(string code)
        {
            var game = GameManager.GetGame(code);
            if (game == null)
            {
                await Clients.Caller.SendAsync("Error", "Gra nie istnieje.");
                return;
            }

            game.CurrentQuestionIndex++;
            await StartNextQuestion(code);
        }

    }
}
