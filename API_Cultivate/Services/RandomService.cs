using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Cultivate.Services
{
    public class RandomService : IRandomService
    {
        private Random _random;

        public RandomService()
        {
            _random = new Random();
        }

        public double NextDouble()
        {
            return _random.NextDouble();
        }
    }
}
