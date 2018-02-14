using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Cultivate.Models;
using Nest;

namespace API_Cultivate.Services
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ElasticClient _client;

        public PlayerRepository()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost.fiddler:9200")).DefaultIndex("players");
            _client = new ElasticClient(settings);
        }

        public async Task<Player> GetPlayer(string id)
        {
            var result = await _client.GetAsync<Player>(id);
            if (result.IsValid)
            {
                return result.Source;
            }

            return null;
        }

        public async Task SavePlayer(Player player)
        {
            var saveResult = await _client.IndexDocumentAsync(player);

            if (!saveResult.IsValid)
            {
                throw new Exception("something bad happened when trying to save player.", saveResult.OriginalException);
            }
        }
    }
}
