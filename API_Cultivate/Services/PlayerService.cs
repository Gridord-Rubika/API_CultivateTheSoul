using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Cultivate.Models;

namespace API_Cultivate.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repository;

        public PlayerService(IPlayerRepository repository)
        {
            _repository = repository;

        }

        public async Task<Player> CreatePlayer(string id)
        {
            var player = new Player { Id = id };
            await _repository.SavePlayer(player);
            return player;
        }

        public async Task<Player> GetPlayer(string id)
        {
            var player = await _repository.GetPlayer(id);
            if (player != null)
            {
                return new Player { Id = id };
            }
            else
            {
                return null;
            }
        }
    }
}
