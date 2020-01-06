using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Models.Commands
{
    [AttributeUsage(System.AttributeTargets.Class)]
    class Commandable : Attribute
    {
        private string _commandName { get; set; }
        public Commandable(string commandName)
        {
            _commandName = commandName;
        }

        public string GetCommandName()
        {
            return _commandName;
        }
    }
}
