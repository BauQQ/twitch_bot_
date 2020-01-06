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
    [Commandable("demo")]
    public class demo : ICommandable
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
            string _ms = "";
            if (c != null && e != null)
            {

                _e = e;
                _c = c;
                _ms = c.respons;
                if (_variableC.CheckThis(_ms))
                {
                    _ms = _variableC.ReplaceThis(_ms, e.ChatMessage.DisplayName);
                }
            }
            return _ms;
        }

        public string Result(OnWhisperReceivedArgs e, Command c)
        {
            return "";
        }
    }
}
