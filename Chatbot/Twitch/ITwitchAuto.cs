using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chatbot.Common.Objects;
using TwitchLib.Client.Events;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;

namespace Chatbot.Models.Twitch
{
    public interface ITwitchAuto
    {
        public void VoidAction(TManager tm);
        public string Result(string e);
        public void Result();
    }
}
