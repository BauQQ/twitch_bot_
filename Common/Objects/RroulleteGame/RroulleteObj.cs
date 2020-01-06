using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common.Objects.RroulleteGame
{
    public class RroulleteObj
    {
        public bool accepted { get; set; } = false; // is this enabled or not
        public string name { get; set; } // Challengers Name true
        public string target { get; set; } // Target Name false
        public int wager { get; set; } // Point wager
        public int gun { get; set; } // Gun size
        public int bullets { get; set; } // Bullets in the gun
        public string turn { get; set; } // whos turn    
        public List<bool> bulletTrain { get; set; } = new List<bool>(); // random bullet listing
        public bool spin { get; set; } = true; // can you spin or not
        public int botLossInstant { get; set; } = 1337; // If you play against the bot, getting this number will win you the game otherwise instant loss.

    }
}
