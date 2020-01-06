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

namespace Chatbot.Models.Commands
{
    public class CommandManager
    {
        public Dictionary<string, ICommandable> _commands = new Dictionary<string, ICommandable>();
        public Dictionary<string, DateTime> _ucd = new Dictionary<string, DateTime>();
        public Dictionary<string, DateTime> _gcd = new Dictionary<string, DateTime>();
        public List<ChatterFormatted> _viewers;
        public TwitchClient _twitch;
        public TwitchAPI _api;
        public OnMessageReceivedArgs _e;
        public OnWhisperReceivedArgs _w;
        public Settings _settings;
        public ChatManager _chatManager;
        public Query _query = new Query();
        public bool _ready = false;

        public CommandManager(ChatManager chatManager)
        {
            _chatManager = chatManager;
            _settings = _chatManager._tmanager._main._settings;
            _twitch = _chatManager._tmanager._client;

            var searchForType = typeof(ICommandable);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => searchForType.IsAssignableFrom(p) && p.IsClass);

            foreach (System.Type type in types)
            {
                _commands.Add(type.ToString(), (ICommandable)Activator.CreateInstance(type));
            }
        }

        public string GetCommand(string name, OnMessageReceivedArgs input, Command cmd)
        {
            /*if (!_ready)
            {
                _chatManager._tmanager.ThreadManager._resetEvents["SpotifyAccessGranted2"].WaitOne();
                _chatManager._tmanager.ThreadManager._resetEvents["Twitch_enabled"].WaitOne();
                _ready = true;
            }*/

            foreach (KeyValuePair<string, ICommandable> dcmd in _commands)
            {
                var key = dcmd.Key.Substring(dcmd.Key.LastIndexOf('.') + 1);
                if (key == name.Replace("!", ""))
                {
                    string _cdKey = input.ChatMessage.DisplayName.ToLower();
                    DateTime compTime = DateTime.Now;
                    if (_ucd.ContainsKey(_cdKey + key) && _ucd[_cdKey + key] > compTime
                        || _gcd.ContainsKey(key) && _gcd[key] > compTime)
                    {
                        return "Come on thats just to fast..";
                    }
                    else
                    {
                        _ucd.Remove(_cdKey + key);
                        _ucd.Add(_cdKey + key, DateTime.Now.AddSeconds(cmd.ucd));
                        _gcd.Remove(key);
                        _gcd.Add(key, DateTime.Now.AddSeconds(cmd.gcd));
                        dcmd.Value.VoidAction(this);
                        if (!String.IsNullOrEmpty(input.ChatMessage.Message))
                        {
                            return dcmd.Value.Result(input, cmd);
                        }
                        else
                        {
                            return dcmd.Value.Result(name);
                        }
                    }
                }
            }
            return "";
        }

        public string internalGetCommand(string cmd, string input)
        {
            string _ms = "";
            foreach (KeyValuePair<string, ICommandable> dcmd in _commands)
            {
                var key = dcmd.Key.Substring(dcmd.Key.LastIndexOf('.') + 1);
                if (key == cmd)
                {
                    if (!String.IsNullOrEmpty(input))
                    {
                        _ms = dcmd.Value.Result(input);
                    }
                    else
                    {
                        _ms = dcmd.Value.Result("");
                    }
                }
            }
            return _ms;
        }

        public string GetWhisperCommand(string name, OnWhisperReceivedArgs input, Command cmd)
        {

            foreach (KeyValuePair<string, ICommandable> dcmd in _commands)
            {
                var key = dcmd.Key.Substring(dcmd.Key.LastIndexOf('.') + 1);
                if (key == name.Replace("!", ""))
                {
                    string _cdKey = input.WhisperMessage.DisplayName.ToLower();
                    DateTime compTime = DateTime.Now;
                    if (_ucd.ContainsKey(_cdKey + key) && _ucd[_cdKey + key] > compTime
                        || _gcd.ContainsKey(key) && _gcd[key] > compTime)
                    {
                        return "Come on thats just to fast..";
                    }
                    else
                    {
                        _ucd.Remove(_cdKey + key);
                        _ucd.Add(_cdKey + key, DateTime.Now.AddSeconds(cmd.ucd));
                        _gcd.Remove(key);
                        _gcd.Add(key, DateTime.Now.AddSeconds(cmd.gcd));
                        dcmd.Value.VoidAction(this);
                        if (!String.IsNullOrEmpty(input.WhisperMessage.Message))
                        {
                            return dcmd.Value.Result(input, cmd);
                        }
                        else
                        {
                            return dcmd.Value.Result(name);
                        }
                    }
                }
            }
            return "";
        }

        public void updateViewersCmdLib(List<ChatterFormatted> viewers)
        {
            _viewers = viewers;
        }
    }
}
