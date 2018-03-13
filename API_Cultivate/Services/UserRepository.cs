using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Cultivate.Models;
using Nest;

namespace API_Cultivate.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ElasticClient _client;

        public UserRepository()
        {
            //TODO remove fiddler when making the build
            var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("users");
            _client = new ElasticClient(settings);
        }

        public async Task<UserRecord> GetUserRecord(string userId)
        {
            var result = await _client.GetAsync<UserRecord>(userId);
            if (result.IsValid)
            {
                return result.Source;
            }

            return null;
        }

        public async Task SaveUserRecord(UserRecord userdRecord)
        {
            var saveResult = await _client.IndexDocumentAsync(userdRecord);

            if (!saveResult.IsValid)
            {
                throw new Exception("something bad happened when trying to save user record.", saveResult.OriginalException);
            }
        }
    }
}
