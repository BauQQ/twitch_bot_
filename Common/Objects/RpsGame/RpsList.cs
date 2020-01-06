using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common.Objects.RpsGame
{
    public class RpsList
    {
        public Dictionary<string, RpsObj> table { get; set; } = new Dictionary<string, RpsObj>(); // Full Rock Paper Siccors challenge list
    }
}
