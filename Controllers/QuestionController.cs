using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebQuizApp.Hubs;
using WebQuizApp.Models;

namespace WebQuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        // Test Database
        private static List<Question> Questions = new List<Question>
        {
            new Question
            {
                Id = 1,
                Text = "What is the capital of France?",
                Options = new List<string> { "Berlin", "Madrid", "Paris", "Rome" },
                CorrectAnswer = "Paris"
            },
            new Question
            {
                Id = 2,
                Text = "What is the largest planet in our solar system?",
                Options = new List<string> { "Earth", "Mars", "Jupiter", "Venus" },
                CorrectAnswer = "Jupiter"
            }
        };

        private static List<Answer> UserAnswers = new List<Answer>();
        private readonly IHubContext<QuizHub> _hubContext;


        public QuestionController(IHubContext<QuizHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult GetQuestions()
        {
            return Ok(Questions);
        }

        [HttpPost("add")]
        public IActionResult AddQuestions([FromBody] Question newQuestion)
        {
            if (newQuestion == null || string.IsNullOrEmpty(newQuestion.Text))
                return BadRequest("Question does not fullfill requirements");

            newQuestion.Id = Questions.Count + 1;
            Questions.Add(newQuestion);
            return Ok(newQuestion);
        }


        [HttpPost("submit")]
        public IActionResult SubmitAnswer([FromBody] Answer answer)
        {
            var question = Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question == null)
            {
                return NotFound("Pytanie nie zostało znalezione");
            }

            UserAnswers.Add(answer);

            bool isCorrect = question.CorrectAnswer == answer.SelectedOption;

            _hubContext.Clients.All.SendAsync("ReceiveAnswer", answer.Username, answer.QuestionId, answer.SelectedOption);

            return Ok(new
            {
                isCorrect,
                correctAnswer = question.CorrectAnswer,
                userAnswer = answer.SelectedOption
            });
        }

        [HttpPut("id")]
        public IActionResult UpdateQuestion(int id,  [FromBody] Question updatedQuestion)
        {
            var question = Questions.FirstOrDefault(q => q.Id == id);
            if(question == null)
            {
                return NotFound("Pytanie nie zostało znalezione.");
            }

            question.Text = updatedQuestion.Text;
            question.Options = updatedQuestion.Options;
            question.CorrectAnswer = updatedQuestion.CorrectAnswer;

            return Ok(question);
        }

        [HttpDelete("id")]
        public IActionResult DeleteQuestion(int id)
        {
            var question = Questions.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                return NotFound("Pytanie nie zostało znalezione.");
            }

            Questions.Remove(question);
            return Ok("Question \"" + question.Text + "\" is removed");
        }

        [HttpGet("answer")]
        public IActionResult GetUserAnswers()
        {
            return Ok(UserAnswers);
        }
    }
}
