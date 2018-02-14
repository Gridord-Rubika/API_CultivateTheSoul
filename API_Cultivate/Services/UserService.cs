using API_Cultivate.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API_Cultivate.Services
{
    public class UserService : IUserService, IDisposable
    {
        private readonly IUserRepository _repository;
        private readonly HashAlgorithm _hash = new SHA256Managed();
        private const string SEED = "mySeed";
        private const string SECRET = "mySecret";
        
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> GetUser(string userId)
        {
            var record = await _repository.GetUserRecord(userId);
            if(record != null)
            {
                return new User { Id = userId };
            }
            else
            {
                return null;
            }
        }

        public async Task<User> CreateUser(string userId, string password)
        {
            var user = new User { Id = userId };

            var record = new UserRecord { Id = userId, PasswordHash = ComputeHash(user, password) };
            await _repository.SaveUserRecord(record);
            return user;
        }

        public async Task<bool> Authenticate(User user, string password)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var record = await _repository.GetUserRecord(user.Id);
            if(record == null)
            {
                return false;
            }

            return record.PasswordHash == ComputeHash(user, password);
        }

        public string GenerateConnectionToken(User user)
        {
            var bytes = Encoding.UTF8.GetBytes(SECRET + user.Id);
            return Convert.ToBase64String(_hash.ComputeHash(bytes));
        }

        public bool VerifyToken(User user, string token)
        {
            return token == GenerateConnectionToken(user);
        }


        private string ComputeHash(User user, string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password + SEED + user.Id);
            return Convert.ToBase64String(_hash.ComputeHash(bytes));
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _hash.Dispose();
                }

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
        // ~UserService() {
        //   // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
        //   Dispose(false);
        // }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
