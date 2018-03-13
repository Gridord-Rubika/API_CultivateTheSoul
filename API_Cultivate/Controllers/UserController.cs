using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Cultivate.Models;
using System.Security.Cryptography;
using API_Cultivate.Services;
using System.Text.RegularExpressions;

namespace API_Cultivate.Controllers
{
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost("{id}")]
        public async Task<IActionResult> Post(string id, [FromBody]string password)
        {
            if (id == "")
                return BadRequest();

            var user = await _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            else if (await _userService.Authenticate(user, password))
            {
                return Ok(_userService.GenerateConnectionToken(user));
            }
            else
            {
                return StatusCode(403);
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]string password)
        {
            if (id == "" || password == "")
            {
                return BadRequest();
            }

            if(id.Length > 30 || !Regex.IsMatch(id, "^[A-Za-z0-9]+$"))
            {
                return StatusCode(403);
            }

            var user = await _userService.GetUser(id);
            if (user == null)
            {
                user = await _userService.CreateUser(id, password);
                return Ok();
            }
            else if (await _userService.Authenticate(user, password))
            {
                return Ok(_userService.GenerateConnectionToken(user));
            }
            else
            {
                return StatusCode(403);
            }

        }

    }
}
