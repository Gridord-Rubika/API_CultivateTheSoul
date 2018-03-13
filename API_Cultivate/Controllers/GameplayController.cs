using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Cultivate.Models;
using API_Cultivate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Cultivate.Controllers
{
    [Produces("application/json")]
    [Route("api/Gameplay/[action]")]
    public class GameplayController : Controller
    {
        private readonly IGameplayService _gameplayService;
        private readonly IUserService _userService;
        private readonly IPlayerService _playerService;

        public GameplayController(IGameplayService gameplayService, IUserService userService, IPlayerService playerService)
        {
            _gameplayService = gameplayService;
            _userService = userService;
            _playerService = playerService;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ClicksCheck(string id, [FromBody] int clicks)
        {
            var validation = await Validate(id);

            if (validation != null)
            {
                return validation;
            }

            Player player = await _playerService.GetPlayer(id);
            if (player != null)
            {
                var result = await _gameplayService.ClicksCheck(player, clicks);
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> BreakThrough(string id)
        {
            var validation = await Validate(id);

            if (validation != null)
            {
                return validation;
            }

            Player player = await _playerService.GetPlayer(id);
            if (player != null)
            {
                var result = await _gameplayService.BreakThrough(player);
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        private async Task<IActionResult> Validate(string id)
        {
            HttpContext.Request.Headers.TryGetValue("x-token", out var tokenValues);

            if (tokenValues.Count != 1)
            {
                return this.StatusCode(403);
            }

            var user = await _userService.GetUser(id);
            if (user == null || !_userService.VerifyToken(user, tokenValues[0]))
            {
                return this.StatusCode(403);
            }

            return null;
        }
    }
}