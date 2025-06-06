namespace WebQuizApp.Models
{
    public class Game
    {
        public string Code { get; set; } = Guid.NewGuid().ToString().Substring(0, 6);
        public List<Question> Questions { get; set; } = new List<Question>();
        public List<Player> Players { get; set; } = new List<Player>();
        public bool IsActive { get; set; } = false;
        public int CurrentQuestionIndex { get; set; } = 0;

    }
}
