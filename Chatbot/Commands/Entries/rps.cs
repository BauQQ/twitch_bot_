using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using Chatbot.Common.Models;
using Chatbot.Common.Objects;
using Chatbot.Common.Objects.RpsGame;
using Chatbot.Common.Actions;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;

namespace Chatbot.Models.Commands.Entries
{
    [Commandable("rps")]
    public class rps : ICommandable
    {
        public OnMessageReceivedArgs _e;
        public Command _c;
        public VariableC _variableC = new VariableC();
        public RpsList _rpsList = new RpsList();
        public CommandManager _cm;
        public Query _query = new Query();
        public Dictionary<string, string> _queryDic;
        public Dictionary<int, string> _GameEnums;

        public void VoidAction(CommandManager cm)
        {
            _cm = cm;
            _GameEnums = new Dictionary<int, string> {
                { 1, "rock"},
                { 2, "paper"},
                { 3, "siccors"}
            };
        }

        public string Result(string e)
        {
            return "";
        }

        public string Result(OnMessageReceivedArgs e, Command c)
        {
            if (!String.IsNullOrEmpty(e.ChatMessage.Message))
            {
                _queryDic = _query.extractQuery(c.respons, e.ChatMessage.Message);
                return CmdProcessor(e.ChatMessage.DisplayName, c);
            }
            else
            {
                return "Uhm me no understand";
            }
        }

        public string Result(OnWhisperReceivedArgs e, Command c)
        {
            if (!String.IsNullOrEmpty(e.WhisperMessage.Message))
            {
                _queryDic = _query.extractQuery(c.respons, e.WhisperMessage.Message);
                return CmdProcessor(e.WhisperMessage.DisplayName, c);
            }
            else
            {
                return "Uhm me no understand";
            }
        }

        public string CmdProcessor(string user, Command c)
        {
            string ms = "";
            switch (_queryDic["${dcmd}"].ToLower())
            {
                case "!rps":
                    ms = GameProcessor(user, c);
                    break;
                case "!rpsaccept":
                    ms = AcceptRps(user);
                    break;
                case "!rpsdeny":
                    ms = DenyRps(user);
                    break;
                case "!rpscancel":
                    ms = CancelRps(user);
                    break;
                default:
                    ms = "You think i can do everything, huh?";
                    break;
            }
            return ms;
        }

        public string GameProcessor(string user, Command c)
        {
            string ms = "";
            string choice = _queryDic["${query}"].ToLower().Trim();
            switch (choice)
            {
                case "rock":
                    ms = PlayerChoice(user, 1);
                    // Choose the rock
                    break;
                case "paper":
                    ms = PlayerChoice(user, 2);
                    // Choose the paper
                    break;
                case "siccors":
                    ms = PlayerChoice(user, 3);
                    // Choose the siccors
                    break;
                default:
                    ms = InitializeGame(user, c);
                    break;
            }
            return ms;
        }

        public string InitializeGame(string user, Command c)
        {
            string target = _queryDic["${query}"].ToLower();
            string _repons = "";
            if (_rpsList.table.ContainsKey(user.ToLower())
                || _rpsList.table.ContainsKey(_queryDic["${query}"].ToLower()))
            {
                _repons = " a game has already been issued by or torwards you.";
            }
            else
            {
                bool ex = false;
                foreach (ChatterFormatted viewer in _cm._viewers)
                {
                    if (_cm._settings.rpsChallengeMe
                        && target == _cm._settings.targetTwitch
                        && target != user.ToLower())
                    {
                        ex = true;
                        break;
                    }
                    else if (viewer.Username.ToLower() == target
                        && viewer.Username.ToLower() != _cm._settings.twitchUsername.ToLower()
                        && target != user.ToLower())
                    {
                        ex = true;
                        break;
                    }
                }

                if (ex)
                {
                    _repons = setupGame(user, c);
                }
                else
                {
                    _repons = " you can not challange that user.";
                }
            }

            return _repons;
        }

        public string setupGame(string user, Command c)
        {
            string _result = "";
            if (!String.IsNullOrEmpty(_queryDic["${query}"])
                && _queryDic["${query}"] != "error"
                && !String.IsNullOrEmpty(_queryDic["${wager}"]))
            {
                string target = _queryDic["${query}"].ToLower();
                int wager = 0;
                int rounds = 1;

                if (_queryDic["${wager}"] != "error"
                    && Int32.Parse(_queryDic["${wager}"]) > 0)
                {
                    wager = Int32.Parse(_queryDic["${wager}"]);
                }

                if (_queryDic["${rounds}"] != "error"
                                    && Int32.Parse(_queryDic["${rounds}"]) > 0)
                {
                    rounds = Int32.Parse(_queryDic["${rounds}"]);
                }

                RpsObj rpsObj = new RpsObj
                {
                    name = user.ToLower(),
                    target = target.ToLower(),
                    wager = wager,
                    rounds = rounds
                };

                for (int i = 0; i < rpsObj.rounds; i++)
                {
                    rpsObj.roundWinners.Add(i + 1, "");
                    rpsObj.roundDic.Add(i + 1, new Dictionary<string, int>() {
                        { user.ToLower(), 0 },
                        { target.ToLower(), 0 }
                    });
                }

                _rpsList.table.Add(user.ToLower(), rpsObj);
                _result = user + " has challenged " + target + " to a game of rock paper siccors for " + wager + " Dubloons";
            }
            else
            {
                _result = "I was unabled to set up the game.";
            }

            return _result;
        }

        public string CancelRps(string user)
        {
            string ms = "";
            if (_rpsList.table.ContainsKey(user.ToLower()))
            {
                if (_rpsList.table[user.ToLower()].accepted)
                {
                    ms = user + " no! you can not back out now.";
                }
                else
                {
                    _rpsList.table.Remove(user.ToLower());
                    ms = user + " your game has been canceled";
                }
            }
            else
            {
                ms = user + " there was not game in your name.";
            }

            return ms;
        }

        public string DenyRps(string user)
        {
            string ms = "";
            bool ex = false;
            string key = "";
            foreach (KeyValuePair<string, RpsObj> entry in _rpsList.table)
            {
                if (entry.Value.target.ToLower() == user.ToLower())
                {
                    ex = true;
                    key = entry.Key;
                    break;
                }
            }


            if (ex)
            {
                if (_rpsList.table[key].accepted)
                {
                    ms = user + " no! you can not back out now.";
                }
                else
                {
                    _rpsList.table.Remove(key);
                    ms = user + " your have denied the game challenge from " + key;
                }
            }
            else
            {
                ms = user + " there was no challenge targeting you.";
            }


            return ms;
        }

        public string AcceptRps(string user)
        {
            string ms = "Unknown";
            bool ex = false;
            string key = "";
            foreach (KeyValuePair<string, RpsObj> entry in _rpsList.table)
            {
                if (entry.Value.target.ToLower() == user.ToLower())
                {
                    ex = true;
                    key = entry.Key;
                    break;
                }
            }

            if (ex)
            {
                if (!_rpsList.table[key].accepted)
                {
                    _rpsList.table[key].accepted = true;
                    ms = _rpsList.table[key].target + " & " + _rpsList.table[key].name + " game has started. Whisper LuffasBot your choice !rps rock/paper/siccors";

                    // Add more logic!
                }
                else
                {
                    ms = user + " there was no game challenge targeting you.";
                }
            }
            else
            {
                ms = user + " there was no game challenge targeting you.";
            }

            return ms;
        }

        public string PlayerChoice(string user, int choice)
        {
            string ms = "";
            bool ex = false;
            string key = "";
            foreach (KeyValuePair<string, RpsObj> entry in _rpsList.table)
            {
                if (entry.Value.target.ToLower() == user.ToLower())
                {
                    ex = true;
                    key = entry.Key;
                    break;
                }
            }

            if (!ex && _rpsList.table.ContainsKey(user.ToLower()))
            {
                ex = true;
                key = user.ToLower();
            }

            if (ex)
            {
                int round = _rpsList.table[key].cRounds;
                if (_rpsList.table[key].roundDic[round].ContainsKey(user.ToLower()))
                {

                    _rpsList.table[key].roundDic[round][user.ToLower()] = choice;

                    if (round == _rpsList.table[key].rounds)
                    {
                        if (!_rpsList.table[key].roundDic[round].ContainsValue(0))
                        {
                            ms = GetRoundWinner(_rpsList.table[key].roundDic[round], _rpsList.table[key].name, _rpsList.table[key].target, round, key, true);
                        }
                    }
                    else
                    {
                        if (!_rpsList.table[key].roundDic[round].ContainsValue(0))
                        {
                            ms = GetRoundWinner(_rpsList.table[key].roundDic[round], _rpsList.table[key].name, _rpsList.table[key].target, round, key);
                        }
                    }
                }

            }

            return ms;
        }


        private string GetRoundWinner(Dictionary<string, int> round, string player1, string player2, int cround, string key, bool end = false)
        {
            //rock 1
            //paper 2
            //siccors 3
            string ms = "";
            _cm._chatManager._tmanager.Send(player1 + ": " + _GameEnums[round[player1.ToLower()]] + " | " + player2 + ": " + _GameEnums[round[player2.ToLower()]]);

            if (end)
            {
                ms = GetWinnerLogic(round[player1.ToLower()], round[player2.ToLower()], player2, player2);
                // _cm._chatManager._tmanager.Send(player1 + ": " + _GameEnums[round[player1.ToLower()]] + " | " + player2 + ": " + _GameEnums[round[player2.ToLower()]]);
            }
            else
            {
                ms = GetWinnerLogic(round[player1.ToLower()], round[player2.ToLower()], player2, player2);
                _rpsList.table[key].cRounds++;
            }

            _rpsList.table.Remove(key);

            return ms;
        }

        private string GetWinnerLogic(int p1, int p2, string player1, string player2)
        {
            string ms = "";

            if (p1 == p2)
            {
                ms = "Draw";
            }
            else
            {
                switch (p1)
                {
                    case 1:
                        if (p2 == 2)
                        {
                            ms = player2 + " wins!";
                        }
                        else
                        {
                            ms = player1 + " wins!";
                        }
                        break;
                    case 2:
                        if (p2 == 3)
                        {
                            ms = player2 + " wins!";
                        }
                        else
                        {
                            ms = player1 + " wins!";
                        }
                        break;
                    case 3:
                        if (p2 == 1)
                        {
                            ms = player2 + " wins!";
                        }
                        else
                        {
                            ms = player1 + " wins!";
                        }
                        break;
                    default:
                        ms = "Unknown numbers error";
                        break;

                }
            }

            return ms;
        }

    }
}
