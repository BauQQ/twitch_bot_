using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Chatbot.Common;
using Chatbot.Common.Objects;
using Chatbot.Common.Models;
using Chatbot.Common.Actions;
using Chatbot.Common.Objects.Viewers;
using Chatbot.Models;
using Chatbot.Models.Chat;
using Chatbot.Models.Commands;
using Chatbot.Models.Twitch;
using Chatbot.Models.Mysql;


namespace Chatbot
{
    public class Main
    {
        public TManager _tManager;
        public MManager _mManager = new MManager();
        public CommandList _commandList = new CommandList();
        public ViewerStruct _viewerStruct = new ViewerStruct();
        public BuildCreditsList _buildCreditsList;
        public LiveViewers _liveViewers = new LiveViewers();
        public Config _config = new Config();
        public bool _isOnline = false;
        public Settings _settings = new Settings();

        public Main()
        {
            //_mManager.Test();
            ThreadManager.Build(ThreadManager.InternalAborting, "InternalAborting");
            PremadeBindings();
            PremadeThreads();
        }

        public void Run()
        {
            //Console.WriteLine("test");
        }

        public void PremadeBindings()
        {
            //Enabled twitch thread binding
            ThreadManager.Bind("SettingsLoaded");
            ThreadManager.Bind("Twitch_enabled");
            ThreadManager.Bind("PopulateLiveViewerList");
            ThreadManager.Bind("LoadUserInformation");
            ThreadManager.Bind("WaitLoadPlaylist");
            ThreadManager.Bind("PopulateDeviceReady");
            ThreadManager.Bind("PopulateDefaultPlaylistsReady");
            ThreadManager.Bind("MmDoLoadCurrentPlaylistUiReload");
        }

        public void PremadeThreads()
        {
            ThreadManager.Build(InterComThread, "InterComThread");
            ThreadManager.Build(StartTwitchThread, "StartTwitchThread");
        }

        public void InterComThread()
        {
            while (true)
            {
                if (new InterCom().IsOnline())
                {
                    _isOnline = true;
                    break;
                }
                Thread.Sleep(100);
            }
            ThreadManager.CloseThread("InterComThread");
        }

        public void StartTwitchThread()
        {
            //ThreadManager._resetEvents["settingsloaded"].WaitOne();
            _tManager = new TManager(this);
        }

    }

}
