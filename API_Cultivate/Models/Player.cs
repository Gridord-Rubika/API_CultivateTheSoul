using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Cultivate.Models
{
    public class Player
    {
        public Player()
        {
            SoulLevel = 1;
            SoulForce = 0;
            LastTimeClickedCheck = DateTime.Now.AddYears(-1);
        }

        public string Id { get; set; }
        public int SoulLevel { get; set; }
        public int SoulForce { get; set; }
        public DateTime LastTimeClickedCheck { get; set; }
    }
}
