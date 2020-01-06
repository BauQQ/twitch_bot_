using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Models.Twitch
{
    [AttributeUsage(System.AttributeTargets.Class)]
    class TwitchAuto : Attribute
    {
        private string _autoName { get; set; }
        public TwitchAuto(string autoName)
        {
            _autoName = autoName;
        }

        public string GetCommandName()
        {
            return _autoName;
        }
    }
}
