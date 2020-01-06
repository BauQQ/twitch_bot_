using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using Chatbot.Common.Models;
using Chatbot.Common.Objects;
using Chatbot.Common.Actions;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;

namespace Chatbot.Models.Commands.Entries
{
    [Commandable("purpose")]
    public class purpose : ICommandable
    {
        public OnMessageReceivedArgs _e;
        public Command _c;
        public VariableC _variableC = new VariableC();
        public List<ChatterFormatted> _viewers;
        public CommandManager _cm;

        public void VoidAction(CommandManager cm)
        {
            _cm = cm;
        }


        public string Result(string e)
        {
            return "";
        }

        public string Result(OnMessageReceivedArgs e, Command c)
        {
            return c.respons.ToString();
        }

        public string Result(OnWhisperReceivedArgs e, Command c)
        {
            return "";
        }
    }
}
