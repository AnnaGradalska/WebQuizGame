namespace WebQuizApp.Models
{
    public class Player
    {
        public string Username { get; set; }
        public int Score { get; set; } = 0;
        public bool HasAnswered { get; set; } = false;
    }
}
