using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using Chatbot.Common;
using Chatbot.Common.Actions;
using Chatbot.Common.Objects;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;

namespace Chatbot.Models.Commands.Entries
{
    [Commandable("berganderf")]
    public class berganderf : ICommandable
    {
        public OnMessageReceivedArgs _e;
        public Command _c;
        public VariableC _variable = new VariableC();
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
            _e = e;
            _c = c;
            string _ms = c.respons;
            if (_variable.CheckThis(_ms))
            {
                _ms = _variable.ReplaceThis(_ms, "Berganderf");
            }

            return _ms;
        }

        public string Result(OnWhisperReceivedArgs e, Command c)
        {
            return "";
        }
    }
}