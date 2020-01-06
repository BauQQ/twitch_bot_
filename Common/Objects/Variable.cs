using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common.Objects
{
    public class Variable
    {
        public bool enabled { get; set; } = true; // is this enabled or not
        public string name { get; set; } // Name
        public string variable { get; set; } //Direct command
        public string description { get; set; }
        public string dataObject { get; set; }
    }
}
