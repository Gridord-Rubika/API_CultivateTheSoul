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
        private readonly IRandomService _randomService;

        public GameplayService(IPlayerRepository repository, IRandomService randomService)
        {
            _repository = repository;
            _randomService = randomService;
        }

        public async Task<int> ClicksCheck(Player player, int clicksDone)
        {
            TimeSpan diff = DateTime.Now - player.LastTimeClickedCheck;

            player.LastTimeClickedCheck = DateTime.Now;

            //Capped at 5 seconds of clicks
            float diffCapped = Math.Clamp((float)diff.TotalSeconds, 0, 5);

            //Capped at 50 clicks per seconds (World Record higher thant that)
            clicksDone = Math.Clamp(clicksDone, 0, (int)(50 * diffCapped));

            //Random formula but a linear one
            int gainedSoulForce = player.SoulLevel * 5 * clicksDone;
            //Random formula but an exponential one
            float maxSoulForce = MathF.Exp(player.SoulLevel) * 5;

            player.SoulForce = Math.Clamp(player.SoulForce + gainedSoulForce, 0, (int)MathF.Round(maxSoulForce));
            await _repository.SavePlayer(player);

            return gainedSoulForce;
        }

        public async Task<bool> BreakThrough(Player player)
        {
            //Random formula but an exponential one
            float maxSoulForce = MathF.Exp(player.SoulLevel) * 5;

            float chanceToBreakThrough = player.SoulForce / MathF.Round(maxSoulForce);
            
            player.SoulForce = 0;

            bool succeeded = false;
            
            if (_randomService.NextDouble() < chanceToBreakThrough)
            {
                player.SoulLevel++;
                succeeded = true;
            }

            await _repository.SavePlayer(player);

            return succeeded;
        }
    }
}
