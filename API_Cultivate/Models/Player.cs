using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Cultivate.Models
{
    public enum CultivationTechniques
    {
        NONE = 0,
        MORTAL,
        EARTH,
        HEAVEN,
        IMMORTAL
    }

    public class Player
    {
        public Player()
        {
            SoulLevel = 1;
            SoulForce = 0;
            SoulStone = 10;
            CultivationTechnique = CultivationTechniques.MORTAL;
            LastTimeClickedCheck = DateTime.Now.AddYears(-1);
            ClicksInLastSecond = 0;
        }

        public string Id { get; set; }
        public int SoulLevel { get; set; }
        public int SoulForce { get; set; }
        public int SoulStone { get; set; }
        public CultivationTechniques CultivationTechnique { get; set; }
        public DateTime LastTimeClickedCheck { get; set; }
        public int ClicksInLastSecond { get; set; }
    }
}
