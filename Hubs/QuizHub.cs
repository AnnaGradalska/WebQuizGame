using Microsoft.AspNetCore.SignalR;

namespace WebQuizApp.Hubs
{
    public class QuizHub : Hub
    {
        public async Task SendAnswer(string username, int questionId, string selectedOption)
        {
            await Clients.All.SendAsync("ReceiveAnswer", username, questionId, selectedOption);
        }
    }
}
