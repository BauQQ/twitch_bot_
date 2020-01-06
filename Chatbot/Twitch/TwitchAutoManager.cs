using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Api;
using TwitchLib.Api.V5.Models.Auth;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;
using Chatbot.Common.Objects;
using Chatbot.Models.Chat;
using Chatbot.Common.Actions;

namespace Chatbot.Models.Twitch
{
    public class TwitchAutoManager
    {
        public Dictionary<string, ITwitchAuto> _autoTwitch = new Dictionary<string, ITwitchAuto>();
        public Dictionary<string, DateTime> _ucd = new Dictionary<string, DateTime>();
        public Dictionary<string, DateTime> _gcd = new Dictionary<string, DateTime>();
        public List<ChatterFormatted> _viewers;
        public TwitchClient _twitch;
        public TwitchAPI _api;
        public OnMessageReceivedArgs _e;
        public Settings _settings;
        public TManager _twitchManager;
        public Query _query = new Query();
        public bool _ready = false;

        public TwitchAutoManager(TManager twitchManager)
        {
            _twitchManager = twitchManager;

            var searchForType = typeof(ITwitchAuto);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => searchForType.IsAssignableFrom(p) && p.IsClass);

            foreach (System.Type type in types)
            {
                _autoTwitch.Add(type.ToString(), (ITwitchAuto)Activator.CreateInstance(type));
            }
        }

        public string GetAutoTwitch(string name)
        {
            foreach (KeyValuePair<string, ITwitchAuto> att in _autoTwitch)
            {
                att.Value.VoidAction(_twitchManager);
                var key = att.Key.Substring(att.Key.LastIndexOf('.') + 1);
                if (key == name)
                {
                    return att.Value.Result(name);
                }
            }
            return "";
        }

        public void GetSendAutoTwitch(string name)
        {
            foreach (KeyValuePair<string, ITwitchAuto> att in _autoTwitch)
            {
                att.Value.VoidAction(_twitchManager);
                var key = att.Key.Substring(att.Key.LastIndexOf('.') + 1);
                if (key == name)
                {
                    att.Value.Result();
                }
            }
        }
    }
}
