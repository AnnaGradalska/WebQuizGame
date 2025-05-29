using Microsoft.AspNetCore.Mvc;
using WebQuizApp.Models;
using WebQuizApp.Services;

namespace WebQuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {

        [HttpPost("start")]
        public IActionResult StartGame([FromBody] List<Question> questions)
        {
            if(questions == null || questions.Count == 0)
            {
                return BadRequest("Lista of questions is empty");
            }

            GameManager.StartGame(questions);

            return Ok(new { GameManager.CurrentGame.GameCode, GameManager.CurrentGame.IsActive });
        }

        [HttpPost("end")]
        public IActionResult EndGame()
        {
            GameManager.EndGame();
            return Ok("The game is finished");
        }

        [HttpGet("status")]
        public IActionResult GetGameStatus()
        {
            return Ok(new
            {
                GameManager.CurrentGame.GameCode,
                GameManager.CurrentGame.IsActive,
                Players = GameManager.CurrentGame.Players.Select(p => new { p.Username, p.Score }),
                CurrentQuestion = GameManager.CurrentGame.Questions.ElementAtOrDefault(GameManager.CurrentGame.CurrentQuestionIndex)?.Text
            });
        }

        [HttpPost("reset-scores")]
        public IActionResult ResetScores()
        {
            GameManager.ResetScores();
            return Ok("Wyniki zostały wyzerowane");
        }
    }
}
