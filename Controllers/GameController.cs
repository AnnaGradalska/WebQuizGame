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

            var game = GameManager.CreateGame(questions);
            return Ok(new {game.Code});
        }

        [HttpPost("end")]
        public IActionResult EndGame([FromQuery] string code)
        {
            var game = GameManager.GetGame(code);
            if (game == null)
            {
                return NotFound("Gra nie istnieje.");
            }

            game.IsActive = true;
            return Ok("The game is finished");
        }

        [HttpGet("status")]
        public IActionResult GetGameStatus([FromQuery] string code)
        {
            var game = GameManager.GetGame(code);
            if(game == null)
            {
                return NotFound("Gra nie istnieje.");
            }
            return Ok(new
            {
                game.Code,
                game.IsActive,
                Players = game.Players.Select(p => new { p.Username, p.Score }),
                CurrentQuestion = game.Questions.ElementAtOrDefault(game.CurrentQuestionIndex)?.Text
            });
        }

        //[HttpPost("reset-scores")]
        //public IActionResult ResetScores()
        //{
        //    GameManager.ResetScores();
        //    return Ok("Wyniki zostały wyzerowane");
        //}
    }
}
