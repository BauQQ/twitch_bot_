using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TwitchLib;
using TwitchLib.Api;
using TwitchLib.Api.Core.Undocumented;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;
using TwitchLib.Api.Helix.Models.Users;
using TwitchLib.Api.V5.Models.Streams;
using TwitchLib.Api.V5.Models.Subscriptions;
using TwitchLib.Api.V5.Models.Channels;
using TwitchLib.Api.V5.Models.Users;
using TwitchLib.Api.V5.Models.Auth;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;
using TwitchLib.Api.Services.Events.FollowerService;
using TwitchLib.PubSub;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication;
using TwitchLib.PubSub.Common;
using TwitchLib.PubSub.Enums;
using TwitchLib.PubSub.Events;
using TwitchLib.PubSub.Models;
using TwitchLib.PubSub.Interfaces;
using Chatbot.Common;
using Chatbot.Common.Models;
using Chatbot.Common.Objects;
using Chatbot.Models.Chat;
using Chatbot.Models.Twitch;
using Chatbot.Common.Objects.Viewers;
using Chatbot;

namespace Chatbot.Models.Twitch
{
    // Following manager is for the twitch client and api
    public class TManager
    {
        public Main _main;
        public ChatManager _cmanager;
        public TwitchAutoManager _tamanager;
        public Settings _settings;
        public Config _config;
        public TwitchClient _client;
        public TwitchAPI _api = new TwitchAPI();
        public bool _q = false;
        //public Logger _logger = new Logger("TwitchManagerLog", false);

        //Initialize the Twitch Manager
        public TManager(Main main)
        {
            _main = main;
            _settings = _main._settings;
            _config = _main._config;
            _cmanager = new ChatManager(this);
            _tamanager = new TwitchAutoManager(this);

            _api.Settings.ClientId = _settings.clientID.ToString();
            _api.Settings.Secret = _settings.clientSecret.ToString();
            _api.V5.Settings.Secret = _settings.clientSecret.ToString();
            _api.V5.Settings.ClientId = _settings.clientID.ToString();
            _api.V5.Settings.AccessToken = _settings.accessToken.ToString();
            _api.Settings.AccessToken = _settings.accessToken.ToString();

            TwitchStarterThreads();

            if (String.IsNullOrEmpty(_settings.targetTwitchId))
            {
                List<string> ulst = new List<string> { _settings.targetTwitch.ToString() };
                _settings.targetTwitchId = _api.V5.Users.GetUsersByNameAsync(ulst).Result.Matches[0].Id;
                //ThreadManager.Build(_main.SaveSettingsThread, "SaveSettingsThread");
            }
        }

        //Start baseline threads
        public void TwitchStarterThreads()
        {
            ThreadManager.Bind("TwitchTokenRefresher");
            ThreadManager.Bind("Client_connect");
            ThreadManager.Build(TwitchTokenRefresher, "TwitchTokenRefresher");
            ThreadManager.Build(Client_connect, "Client_connect");
        }

        //Twitch Token Refresher
        public async void TwitchTokenRefresher()
        {
            if (!_q)
            {
                RefreshResponse v = await Task.Run(() => RefreshTokenCall()).ConfigureAwait(true);
                _settings.refreshToken = v.RefreshToken;
                _settings.accessToken = v.AccessToken;
                //ThreadManager.Build(_main.SaveSettingsThread, "SaveSettingsThread");
                _q = true;
                ThreadManager.WakeUp("Client_connect");
                ThreadManager.CloseThread("TwitchTokenRefresher");
            }
        }

        private RefreshResponse RefreshTokenCall()
        {
            return _api.V5.Auth.RefreshAuthTokenAsync(_settings.refreshToken, _settings.clientSecret, _settings.clientID).Result;
        }

        //Connect the client to the target twitch
        private void Client_connect()
        {
            ThreadManager._resetEvents["client_connect"].WaitOne();
            ConnectionCredentials credentials = new ConnectionCredentials(_settings.twitchUsername.ToString(), _settings.accessToken.ToString());
            _client = new TwitchClient();
            _cmanager = new ChatManager(this);
            _client.Initialize(credentials, _settings.targetTwitch);
            _client.OnMessageReceived += _cmanager.ReadChat;
            _client.OnUserJoined += ClientUserJoined;
            _client.OnWhisperReceived += _cmanager.ReadWhisper;
            _client.OnUserLeft += ClientOnUserLeft;
            _client.OnWhisperSent += ClientOnWhisperSent;
            _client.Connect();

            ThreadManager.WakeUp("Twitch_enabled");

            ThreadManager.Build(GetLiveViewers, "GetLiveViewers");
        }

        //Client on joining Channel
        private void Client_OnJoin(object sender, OnJoinedChannelArgs e)
        {
            _client.SendMessage(_settings.targetTwitch, String.Format("I have arrived"));
        }

        //Test default chat
        public void TestChat()
        {
            Send(String.Format("Hey im a new bot!"));
            //UserSendWhisper("luffaow", "test");
        }

        //On users joining channel
        public void ClientUserJoined(object sender, OnUserJoinedArgs e)
        {
            Console.WriteLine("Joined: "+e.Username.ToString());
            //Send(e.Username.ToString()+" joined the channel.");
            Dictionary<string, Viewer> _liveViewersDump = new Dictionary<string, Viewer>(_main._liveViewers.table);
            foreach (KeyValuePair<string, Viewer> viewer in _liveViewersDump)
            {
                if (viewer.Key == e.Username)
                {
                    if (viewer.Value.twitchAuto != null)
                    {
                        foreach (string att in viewer.Value.twitchAuto)
                        {
                            _tamanager.GetSendAutoTwitch(att);
                        }
                    }
                }
            }

        }

        //on users leaving channel
        public void ClientOnUserLeft(object sender, OnUserLeftArgs e)
        {
            //Console.WriteLine("Left: " + e.Username.ToString()); 
            //Send(e.Username.ToString() + " left us.");
            if (_main._liveViewers.table.ContainsKey(e.Username))
            {
                _main._liveViewers.table.Remove(e.Username);
            }
            //_logger.Push(e.Users.ToString());

            // UsersMovementBox
        }

        public void UserSendWhisper(string target, string message)
        {
            //Console.WriteLine(target+" "+message);
            //_client.SendWhisper(String.Format(target), String.Format(message), true);
            //Console.WriteLine(_client.IsConnected);
            //_client.SendWhisper("spectrapulse", "something long for a test message");
            //_client.ReplyToLastWhisper("something long for a test message");
            //_client.InvokeWhisperSent(String.Format("luffaow"), String.Format("luffasbot"), String.Format("something long for a test message"));
        }

        public void ClientOnWhisperSent(object sender, OnWhisperSentArgs e)
        {
            Console.WriteLine(e.Message);
            //_client.SendWhisper(String.Format(target), String.Format(message), true);
        }

        //Default send function
        public void Send(string message)
        {
            if (_client.JoinedChannels.Count >= 1)
            {
                _client.SendMessage(_settings.targetTwitch, message);
            }
        }

        //Rolling thread for liveViewers
        public async void GetLiveViewers()
        {
            while (true)
            {
                var v = await _api.Undocumented.GetChattersAsync(_settings.targetTwitch).ConfigureAwait(true);
                LiveViewerProcessor(v);
                _cmanager._commandManager.updateViewersCmdLib(v);
                Thread.Sleep(_settings.viewerInterval);
            }
        }

        //Load All the viewers in
        public void LiveViewerProcessor(List<ChatterFormatted> viewers)
        {
            LiveViewers _dupLiveViewer = new LiveViewers();
            foreach (ChatterFormatted viewer in viewers)
            {
                Viewer LiveViewer = new Viewer
                {
                    username = viewer.Username,
                    level = (int)viewer.UserType

                };

                _main._mManager.AddViewer(LiveViewer.username, LiveViewer.level, LiveViewer.subscriber ? 1 : 0);

                if (!_main._viewerStruct.whitelist.ContainsKey(LiveViewer.username)
                    && !_main._viewerStruct.blacklist.ContainsKey(LiveViewer.username))
                {
                    _main._viewerStruct.whitelist.Add(LiveViewer.username, LiveViewer);
                }

                if (!_main._liveViewers.table.ContainsKey(LiveViewer.username))
                {
                    if (_main._viewerStruct.whitelist.ContainsKey(LiveViewer.username))
                    {
                        _main._liveViewers.table.Add(LiveViewer.username, _main._viewerStruct.whitelist[LiveViewer.username]);
                    }
                    else
                    {
                        _main._liveViewers.table.Add(LiveViewer.username, _main._viewerStruct.blacklist[LiveViewer.username]);
                    }
                }
            }

            // FIX at later time.

            //ThreadManager.Build(_main.SaveViewerStructThread, "SaveViewerStructThread");
            //ThreadManager.WakeUp("PopulateLiveViewerList");
        }

    }
}
