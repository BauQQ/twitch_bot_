using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chatbot.Common.Models;
using Chatbot.Common.Objects;
using Chatbot.Models.Twitch;
using Chatbot;
using System.Text.RegularExpressions;
using TwitchLib.Api;
using TwitchLib.Api.V5.Models.Auth;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;
using Chatbot.Models.Commands;

namespace Chatbot.Models.Chat
{
    public class ChatManager
    {
        public TManager _tmanager;
        public OnMessageReceivedArgs _e;
        public OnWhisperReceivedArgs _w;
        public CommandList _commandList;
        public CommandManager _commandManager;
        //public Logger _logger = new Logger("ChatManagerLog", false);

        public ChatManager(TManager tmanager)
        {
            _tmanager = tmanager;
            _commandList = _tmanager._main._commandList;
            _commandManager = new CommandManager(this);
        }

        public void ReadChat(object sender, OnMessageReceivedArgs e)
        {
            _e = e;
            if (e.ChatMessage.DisplayName.ToString() != _tmanager._settings.twitchUsername.ToString())
            {
                //_tmanager.postToInterface(e.ChatMessage.DisplayName.ToString() + ": " + e.ChatMessage.Message.ToString());
                string el = checkForCommandInput(e.ChatMessage.Message.ToString());
                if (!String.IsNullOrEmpty(el))
                {
                    _tmanager.Send(el);
                }
            }
        }
        public void ReadWhisper(object sender, OnWhisperReceivedArgs w)
        {

            //_tmanager.Send(w.WhisperMessage.DisplayName+" whispered me: "+ w.WhisperMessage.Message);
            _w = w;
            if (w.WhisperMessage.DisplayName.ToString() != _tmanager._settings.twitchUsername.ToString())
            {
                //_tmanager.postToInterface(e.ChatMessage.DisplayName.ToString() + ": " + e.ChatMessage.Message.ToString());
                string wl = checkForWhisperCommandInput(w.WhisperMessage.Message.ToString());
                if (!String.IsNullOrEmpty(wl))
                {
                    _tmanager.Send(wl);
                }
            }
        }
        public string checkForWhisperCommandInput(string w)
        {
            bool pass = false;
            foreach (KeyValuePair<string, Command> cmd in _commandList.table)
            {
                if (!cmd.Value.enabled)
                {
                    continue;
                }

                foreach (string cmdAlias in cmd.Value.cmdAliases)
                {

                    var fsi = w.IndexOf(" ");
                    if (fsi > 0)
                    {
                        string firstE = w.Substring(0, fsi);
                        string fci = firstE.Substring(0, 1);
                        if (pass == false && fci == "!")
                        {
                            pass = Regex.IsMatch(firstE, string.Format(@"\b{0}\b", Regex.Escape(cmdAlias)));
                        }
                    }
                    else
                    {
                        string fci = w.Substring(0, 1);
                        if (pass == false && fci == "!")
                        {
                            pass = Regex.IsMatch(w, string.Format(@"\b{0}\b", Regex.Escape(cmdAlias)));
                        }
                    }

                }

                if (pass)
                {
                    if (cmd.Value.locked.Count <= 0)
                    {
                        pass = false;
                        return _commandManager.GetWhisperCommand(cmd.Value.dcmd, _w, cmd.Value);
                    }
                    else
                    {


                        if (cmd.Value.locked.Contains(_w.WhisperMessage.DisplayName.ToString().ToLower()))
                        {
                            pass = false;
                            return _commandManager.GetWhisperCommand(cmd.Value.dcmd, _w, cmd.Value);
                        }
                        else
                        {
                            pass = false;
                            return "Sorry this command is not available to you.";
                        }
                    }
                }
            }
            pass = false;
            return "";
        }

        public string checkForCommandInput(string e)
        {
            bool pass = false;
            foreach (KeyValuePair<string, Command> cmd in _commandList.table)
            {
                if (!cmd.Value.enabled)
                {
                    continue;
                }

                foreach (string cmdAlias in cmd.Value.cmdAliases)
                {

                    var fsi = e.IndexOf(" ");
                    if (fsi > 0)
                    {
                        string firstE = e.Substring(0, fsi);
                        string fci = firstE.Substring(0, 1);
                        if (pass == false && fci == "!")
                        {
                            pass = Regex.IsMatch(firstE, string.Format(@"\b{0}\b", Regex.Escape(cmdAlias)));
                        }
                    }
                    else
                    {

                        string fci = e.Substring(0, 1);
                        if (pass == false && fci == "!")
                        {
                            pass = Regex.IsMatch(e, string.Format(@"\b{0}\b", Regex.Escape(cmdAlias)));
                        }
                    }

                }


                if (pass)
                {
                    if (cmd.Value.locked.Count <= 0)
                    {
                        pass = false;
                        return _commandManager.GetCommand(cmd.Value.dcmd, _e, cmd.Value);
                    }
                    else
                    {


                        if (cmd.Value.locked.Contains(_e.ChatMessage.DisplayName.ToString().ToLower()))
                        {
                            pass = false;
                            return _commandManager.GetCommand(cmd.Value.dcmd, _e, cmd.Value);
                        }
                        else
                        {
                            pass = false;
                            return "Sorry this command is not available to you.";
                        }
                    }
                }
            }
            pass = false;
            return "";
        }

    }
}
