using API_Cultivate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Cultivate.Services
{
    public class GameplayService : IGameplayService
    {
        private readonly IPlayerRepository _repository;

        public GameplayService(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Cultivate(Player player)
        {
            int gainedSoulForce = 0;

            TimeSpan diff = DateTime.Now - player.LastTimeClickedCheck;

            if (diff.TotalMilliseconds >= 1000)
            {
                player.LastTimeClickedCheck = DateTime.Now;
                player.ClicksInLastSecond = 0;
            }

            if (player.ClicksInLastSecond < 4)
            {
                player.SoulForce++;
                player.ClicksInLastSecond++;
                await _repository.SavePlayer(player);
            }

            return gainedSoulForce;
        }
    }
}
