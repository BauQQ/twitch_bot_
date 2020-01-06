using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common.Objects
{
    public class Command
    {
        public bool enabled { get; set; } // is this enabled or not
        public string name { get; set; } // Name
        public string dcmd { get; set; } //Direct command
        public List<string> cmdAliases { set; get; } // Command Aliases for the same command
        public int userLevel { get; set; } // Required userlevel to activate the command
        public int ucd { get; set; } // User Cooldown timer
        public int gcd { get; set; } // Global Cooldown timer
        public int cost { get; set; } // Point it's gonna cost
        public string respons { get; set; } // Appropriate respons
        public List<string> locked { get; set; } // Is this locked to specific users
    }
}
