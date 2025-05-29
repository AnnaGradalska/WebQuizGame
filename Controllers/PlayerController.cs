using Microsoft.AspNetCore.Mvc;
using WebQuizApp.Models;
using WebQuizApp.Services;

namespace WebQuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {

        [HttpPost("join")]
        public IActionResult JoinGame([FromBody] Player player)
        {
            if (!GameManager.CurrentGame.IsActive)
            {
                return BadRequest("Gra nie jest jeszcze aktywna");
            }

            GameManager.CurrentGame.Players.Add(new Player { Username = player.Username});
            return Ok("Player added to the game.");
        }

        [HttpPost("answer")]
        public IActionResult SubmitAnswer([FromBody] Answer answer)
        {
            var player = GameManager.CurrentGame.Players.FirstOrDefault(p => p.Username == answer.Username);
            if (player == null)
                return NotFound("Gracz nie został znaleziony.");

            var question = GameManager.CurrentGame.Questions.ElementAtOrDefault(GameManager.CurrentGame.CurrentQuestionIndex);
            if (question == null)
                return BadRequest("Pytanie nie istnieje.");

            if (player.HasAnswered)
                return BadRequest("Gracz już odpowiedział na to pytanie.");

            player.HasAnswered = true;

            if (answer.SelectedOption == question.CorrectAnswer)
                player.Score += 100;

            return Ok(new { player.Username, player.Score });
        }
    }
}
