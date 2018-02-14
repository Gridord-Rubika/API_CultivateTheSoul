using API_Cultivate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Cultivate.Services
{
    public interface IGameplayService
    {
        Task<int> Cultivate(Player player);
    }
}
