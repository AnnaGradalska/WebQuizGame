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
        public IActionResult JoinGame([FromQuery] string code,[FromBody] Player player)
        {
            var game = GameManager.GetGame(code);
            if (game == null || !game.IsActive)
            {
                return BadRequest("Gra o podanym kodzie nie jest aktywna lub nie istnieje");
            }

            if (game.Players.Any(p => p.Username == player.Username))
            {
                return BadRequest("Gracz o podanej nazwie istnieje!");
            }

            game.Players.Add(new Player { Username = player.Username });

            return Ok("Player added to the game.");
        }

        [HttpPost("answer")]
        public IActionResult SubmitAnswer([FromQuery] string code, [FromBody] Answer answer)
        {
            var game = GameManager.GetGame(code);
            if(game == null)
            {
                return BadRequest("Gra nie istnieje");
            }

            var player = game.Players.FirstOrDefault(p => p.Username == answer.Username);
            if (player == null)
                return NotFound("Gracz nie został znaleziony.");

            var question = game.Questions.ElementAtOrDefault(game.CurrentQuestionIndex);
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
