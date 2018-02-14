using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using API_Cultivate.Models;
using API_Cultivate.Services;

namespace API_Cultivate.Controllers
{
    [Produces("application/json")]
    [Route("api/Player")]
    public class PlayerController : Controller
    {

        private readonly ElasticClient _client = new ElasticClient(new ConnectionSettings().DefaultIndex("players"));
        private readonly IUserService _userService;
        private readonly IPlayerService _playerService;

        public PlayerController(IUserService userService, IPlayerService playerService)
        {
            _userService = userService;
            _playerService = playerService;
        }

        // GET: api/GamePlay/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var validation = await Validate(id);

            if(validation != null)
            {
                return validation;
            }
            
            var player = await _playerService.GetPlayer(id);

            if(player != null)
            {
                return Ok(player);
            }   
            else
            {
                return NotFound();
            }
        }
        
        // PUT: api/GamePlay/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Player value)
        {
            var validation = await Validate(id);

            if (validation != null)
            {
                return validation;
            }

            var player = await _playerService.GetPlayer(id);
            if (player == null)
            {
                player = await _playerService.CreatePlayer(id);
                return Ok();
            }
            else
            {
                return StatusCode(403);
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
