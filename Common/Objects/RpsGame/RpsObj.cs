using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common.Objects.RpsGame
{
    public class RpsObj
    {
        public bool accepted { get; set; } = false; // is this enabled or not
        public string name { get; set; } // Challengers Name true
        public string target { get; set; } // Target Name false
        public int wager { get; set; } // Point wager
        public Dictionary<int, string> roundWinners { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, Dictionary<string, int>> roundDic { get; set; } = new Dictionary<int, Dictionary<string, int>>();
        public int rounds { get; set; } = 1;
        public int cRounds { get; set; } = 1;
        
    }
}
