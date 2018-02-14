using API_Cultivate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Cultivate.Services
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayer(string id);
        Task SavePlayer(Player player);
    }
}
