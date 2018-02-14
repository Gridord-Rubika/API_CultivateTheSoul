using API_Cultivate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Cultivate.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string userId);
        Task<User> CreateUser(string userId, string password);
        Task<bool> Authenticate(User user, string password);
        string GenerateConnectionToken(User user);
        bool VerifyToken(User user, string token);
    }
}
