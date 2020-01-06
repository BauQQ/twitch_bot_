using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chatbot.Common.Objects;
using TwitchLib.Client.Events;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;

namespace Chatbot.Models.Commands
{
    public interface ICommandable
    {
        public void VoidAction(CommandManager cm);
        public string Result(string e);
        public string Result(OnMessageReceivedArgs e, Command c);
        public string Result(OnWhisperReceivedArgs e, Command c);

    }
}
