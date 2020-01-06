using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;
using Chatbot.Common;
using Chatbot.Common.Actions;
using Chatbot.Common.Models;
using Chatbot.Common.Objects;
using Chatbot.Common.Objects.RroulleteGame;

namespace Chatbot.Models.Commands.Entries
{
    [Commandable("rroullete")]
    public class rroullete : ICommandable
    {
        public OnMessageReceivedArgs _e;
        public Query _query = new Query();
        public RroulleteList _RroulleteList = new RroulleteList();
        public string _reason = "";
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
            string _ms = "Nope i can't do that....";
            string _user = e.ChatMessage.DisplayName;

            Dictionary<string, string> _queryDic = _query.extractQuery(c.respons, e.ChatMessage.Message);
            switch (_queryDic["${dcmd}"].ToLower())
            {
                case "!rraccept":
                    _ms = "";
                    AcceptRoullete(_user);

                    break;
                case "!rrdeny":
                    _ms = "";
                    DenyRoullete(_user);

                    break;
                case "!rrshoot":
                    _ms = "";
                    ShootRoullete(_user);

                    break;
                case "!rrcancel":
                    CancelRoullete(_user);
                    _ms = "";

                    break;
                case "!rradmcancel":
                    _ms = "";
                    AdmCancelGame(_queryDic);

                    break;
                case "!rrspin":
                    _ms = "";
                    SpinBarrel(_user);

                    break;
                case "!rroullete":
                    bool se = IntiChallenge(e.ChatMessage.DisplayName.ToLower(), _queryDic);
                    if (se)
                    {
                        _ms = CheckSetup(e.ChatMessage.DisplayName, _queryDic["${target}"], _queryDic["${wager}"]);
                    }
                    else
                    {
                        _ms = _user + _reason;
                    }

                    break;
                default:
                    _ms = "Sorry come again?";
                    // I dunno..
                    break;
            }

            return _ms;
        }

        private bool IntiChallenge(string user, Dictionary<string, string> setupDic)
        {
            if (!_RroulleteList.table.ContainsKey(user.ToLower()))
            {
                bool ex = false;
                foreach (ChatterFormatted viewer in _cm._viewers)
                {
                    if (_cm._settings.rRoulleteChallengeMe == true
                        && setupDic["${target}"].ToLower() == _cm._settings.targetTwitch.ToLower()
                        && setupDic["${target}"].ToLower() != user.ToLower())
                    {
                        ex = true;
                        break;
                    }
                    else if (viewer.Username.ToLower() == setupDic["${target}"].ToLower()
                        && viewer.Username.ToLower() != _cm._settings.twitchUsername.ToLower()
                        && setupDic["${target}"].ToLower() != user.ToLower())
                    {
                        ex = true;
                        break;
                    }

                }
                if (ex)
                {
                    return SetupChallenge(user, setupDic);
                }
                else
                {
                    _reason = " you can not challenge that user.";
                    return false;
                }
            }
            else
            {
                _reason = " you can only have one challenge running.. !rrcancel it before trying again.";
                return false;
            }
        }

        private bool SetupChallenge(string user, Dictionary<string, string> setupDic)
        {
            if (_RroulleteList.table.ContainsKey(setupDic["${target}"].ToLower())
                || _RroulleteList.table.ContainsKey(user.ToLower()))
            {
                _reason = " a challenge has already been issued by or torwards you.";
                return false;
            }
            else
            {
                if (setupDic.ContainsKey("${gunsize}") && setupDic.ContainsKey("${bullets}") && setupDic.ContainsKey("${wager}"))
                {
                    if (String.IsNullOrEmpty(setupDic["${gunsize}"])
                        || String.IsNullOrWhiteSpace(setupDic["${gunsize}"])
                        || setupDic["${gunsize}"] == "error"
                        || Int32.Parse(setupDic["${gunsize}"]) <= 0)
                    {
                        _reason = " please don't mess with my gun.";
                        return false;
                    }

                    if (String.IsNullOrEmpty(setupDic["${bullets}"])
                        || String.IsNullOrWhiteSpace(setupDic["${bullets}"])
                        || setupDic["${bullets}"] == "error"
                        || Int32.Parse(setupDic["${bullets}"]) <= 0)
                    {
                        _reason = " please don't mess with my gun.";
                        return false;
                    }

                    if (String.IsNullOrEmpty(setupDic["${wager}"])
                        || String.IsNullOrWhiteSpace(setupDic["${wager}"])
                        || setupDic["${wager}"] == "error"
                        || Int32.Parse(setupDic["${wager}"]) <= 0)
                    {
                        _reason = " could you bet something, otherwise it's not fun.";
                        return false;
                    }

                    if (Int32.Parse(setupDic["${gunsize}"]) <= Int32.Parse(setupDic["${bullets}"]))
                    {
                        _reason = " please don't mess with my gun.";
                        return false;
                    }
                }

                RroulleteObj Rroullete = new RroulleteObj
                {
                    name = user,
                    target = setupDic["${target}"],
                    gun = Int32.Parse(setupDic["${gunsize}"]),
                    bullets = Int32.Parse(setupDic["${bullets}"]),
                    wager = Int32.Parse(setupDic["${wager}"]),
                    bulletTrain = CreateBulletTrain(user, setupDic)
                };
                Rroullete.turn = WhoStarts(Rroullete.name, Rroullete.target);
                _RroulleteList.table.Add(user.ToLower(), Rroullete);
                // Add more code here LUFFA!
                return true;
            }
        }

        private string WhoStarts(string user, string target)
        {
            int n = Rng(0, 100);
            if (n > 50)
            {
                return user.ToLower();
            }
            else
            {
                return target.ToLower();
            }
        }


        private List<T> ShuffleTrain<T>(List<T> input)
        {
            List<T> rngList = new List<T>();
            Random rng = new Random();
            while (input.Count > 0)
            {
                int r = rng.Next(0, input.Count);
                rngList.Add(input[r]);
                input.RemoveAt(r);
            }

            return rngList;
        }

        private void SpinBarrel(string user)
        {
            bool ex = false;
            string key = "";
            foreach (KeyValuePair<string, RroulleteObj> entry in _RroulleteList.table)
            {
                if (entry.Value.target.ToLower() == user.ToLower()
                    || entry.Key.ToLower() == user.ToLower()
                    && entry.Value.turn.ToLower() == user.ToLower())
                {
                    ex = true;
                    key = entry.Key;
                    break;
                }
            }

            if (ex)
            {
                if (_RroulleteList.table[key].spin)
                {
                    //Random rng = new Random();
                    //_RroulleteList.table[key].bulletTrain = _RroulleteList.table[key].bulletTrain.OrderBy(x => rng.Next()).ToDictionary(item => item.Key, item => item.Value);
                    _RroulleteList.table[key].bulletTrain = ShuffleTrain<bool>(_RroulleteList.table[key].bulletTrain);
                    _RroulleteList.table[key].spin = false;
                    Send(user + " just spun the gunbarrel..");
                }
                else
                {
                    Send(user + " you can only spin it once pr turn.");
                }
            }
            else
            {
                Send(user + " there was no challenge with your name on");
            }

            /*
            foreach (KeyValuePair<int, bool> ent in _RroulleteList.table[key].bulletTrain)
            {
                Console.WriteLine(ent.Value.ToString());
            }*/
        }

        public int Rng(int s, int e)
        {
            Random rng = new Random();
            int key = rng.Next(s, e);
            return key;
        }

        private List<bool> CreateBulletTrain(string user, Dictionary<string, string> setupDic)
        {
            string _user = user;
            List<bool> train = new List<bool>();
            int t = Int32.Parse(setupDic["${gunsize}"]);
            int n = Int32.Parse(setupDic["${bullets}"]);
            for (int i = 0; i < t; i++)
            {
                train.Add(false);
            }

            for (int i = 0; i < n; i++)
            {
                int key = Rng(0, t);
                if (train[key] == true)
                {
                    i--;
                }
                else
                {
                    train[key] = true;
                }
            }

            /*
              foreach (KeyValuePair<int, bool> ent in train)
              {
                  Console.WriteLine(ent.Value.ToString());
              }*/
            return train;
        }

        private string CheckSetup(string user, string target, string wager)
        {
            if (wager == "error")
            {
                Send("A Russian Roullete has been setup between " + user + " and " + target + " for 0 dubloons.");
                return "Do you !rraccept or !rrdeny " + target + "?";
            }
            else
            {
                Send("A Russian Roullete has been setup between " + user + " and " + target + " for " + wager + " dubloons.");
                return "Do you !rraccept or !rrdeny " + target + "?";
            }
        }

        private void CancelRoullete(string user)
        {
            if (_RroulleteList.table.ContainsKey(user.ToLower()))
            {
                if (_RroulleteList.table[user.ToLower()].accepted)
                {
                    Send(user + " no! you can not back out now.");
                }
                else
                {
                    _RroulleteList.table.Remove(user.ToLower());
                    Send(user + " your challenge has been canceled.");
                }
            }
            else
            {
                Send(user + " there was no challenge in your name.");

            }
        }

        private void DenyRoullete(string target)
        {
            bool ex = false;
            string key = "";
            foreach (KeyValuePair<string, RroulleteObj> entry in _RroulleteList.table)
            {
                if (entry.Value.target.ToLower() == target.ToLower())
                {
                    ex = true;
                    key = entry.Key;
                    break;
                }
            }

            if (ex)
            {
                if (_RroulleteList.table[key].accepted)
                {
                    Send(target + " no! you can not back out now.");
                }
                else
                {
                    _RroulleteList.table.Remove(key);
                    Send(target + " you have denied the challenge from " + key);
                }
            }
            else
            {
                Send(target + " there was no challenge targeting you.");
            }
        }

        public void AcceptRoullete(string target)
        {
            bool ex = false;
            string key = "";
            foreach (KeyValuePair<string, RroulleteObj> entry in _RroulleteList.table)
            {
                if (entry.Value.target.ToLower() == target.ToLower())
                {
                    ex = true;
                    key = entry.Key;
                    break;
                }
            }

            if (ex)
            {
                if (!_RroulleteList.table[key].accepted)
                {
                    _RroulleteList.table[key].accepted = true;
                    Send(target + " has accepted your challenge " + key);
                    Send(_RroulleteList.table[key].turn + " starts!");
                }
                else
                {
                    Send(target + " there was no challenge targeting you.");

                }
            }
            else
            {
                Send(target + " there was no challenge targeting you.");
            }

        }

        #region Deprecated
        /*
        public void shootRoullete(string user)
        {
            bool death = false;
            bool ex = false;
            string key = "";
            foreach (KeyValuePair<string, Rroullete> entry in _RroulleteList.table)
            {
                if (entry.Value.target.ToLower() == user.ToLower()
                    || entry.Key.ToString().ToLower() == user.ToLower())
                {
                    ex = true;
                    key = entry.Key.ToString();
                    break;
                }
            }

            if (ex)
            {
                if (_RroulleteList.table[key].turn.ToLower() == user.ToLower())
                {
                    var first = _RroulleteList.table[key].bulletTrain.First();
                    int bulletkey = first.Key;
                    var highKey = _RroulleteList.table[key].bulletTrain.Keys.Max() + 2;
                    if (_RroulleteList.table[key].bulletTrain[bulletkey])
                    {
                        Send(_RroulleteList.table[key].turn + " you're a dumbass, you shot yourself..");
                        death = true;
                    }
                    else
                    {
                        Send(_RroulleteList.table[key].turn + " you survived this round..");
                    }

                    _RroulleteList.table[key].bulletTrain.Remove(bulletkey);
                    _RroulleteList.table[key].bulletTrain.Add(highKey, false);
                    _RroulleteList.table[key].bulletTrain = _RroulleteList.table[key].bulletTrain.OrderBy(u => u.Key).ToDictionary(f => f.Key, ff => ff.Value);
                    // !Rroullete spectrapulse 7 2 300
                    Console.WriteLine(highKey);

                    foreach (KeyValuePair<int, bool> ent in _RroulleteList.table[key].bulletTrain)
                    {
                        Console.WriteLine(ent.Value.ToString());
                    }

                    if (death)
                    {
                        winContinue(key, _RroulleteList.table[key].turn);
                        // GAME ENDS HERE
                    }
                    else
                    {
                        if (_RroulleteList.table[key].turn.ToLower() == _RroulleteList.table[key].name.ToLower())
                        {
                            _RroulleteList.table[key].turn = _RroulleteList.table[key].target;
                        }
                        else
                        {
                            _RroulleteList.table[key].turn = _RroulleteList.table[key].name;
                        }
                    }

                }
                else
                {
                    Send(user + " it is not your turn.");
                }
            }
            else
            {
                Send(user + " there was no challenge with your name on.");
            }
        }
        */
        #endregion

        public void ShootRoullete(string user)
        {
            bool death = false;
            bool ex = false;
            string key = "";
            foreach (KeyValuePair<string, RroulleteObj> entry in _RroulleteList.table)
            {
                if (entry.Value.target.ToLower() == user.ToLower()
                    || entry.Key.ToLower() == user.ToLower())
                {
                    ex = true;
                    key = entry.Key;
                    break;
                }
            }

            if (ex && _RroulleteList.table[key].accepted)
            {
                if (_RroulleteList.table[key].turn.ToLower() == user.ToLower())
                {
                    //var first = _RroulleteList.table[key].bulletTrain.IndexOf(_RroulleteList.table[key].bulletTrain.Min());
                    var first = 0;
                    var last = _RroulleteList.table[key].bulletTrain.Count;
                    _RroulleteList.table[key].spin = true;
                    if (_RroulleteList.table[key].bulletTrain[first])
                    {
                        Send(_RroulleteList.table[key].turn + " you're a dumbass, you shot yourself..");
                        death = true;
                    }
                    else
                    {
                        Send(_RroulleteList.table[key].turn + " you survived this round..");
                    }

                    _RroulleteList.table[key].bulletTrain.RemoveAt(first);
                    _RroulleteList.table[key].bulletTrain.Insert(last - 1, false);

                    // !Rroullete spectrapulse 7 2 300

                    if (death)
                    {
                        WinContinue(key, _RroulleteList.table[key].turn);
                        // GAME ENDS HERE
                    }
                    else
                    {
                        if (_RroulleteList.table[key].turn.ToLower() == _RroulleteList.table[key].name.ToLower())
                        {
                            _RroulleteList.table[key].turn = _RroulleteList.table[key].target;
                        }
                        else
                        {
                            _RroulleteList.table[key].turn = _RroulleteList.table[key].name;
                        }
                    }

                }
                else
                {
                    Send(user + " it is not your turn.");
                }
            }
            else
            {
                Send(user + " there was no challenge with your name on.");
            }
        }

        public void WinContinue(string key, string dead)
        {
            string _winner = "";
            string _dead = dead;
            if (_RroulleteList.table[key].turn.ToLower() == _RroulleteList.table[key].name.ToLower())
            {
                _winner = _RroulleteList.table[key].target;
            }
            else
            {
                _winner = _RroulleteList.table[key].name;
            }

            Send(_winner + " won this game.");
            Send("!addpoints " + _winner + " 1");
            _RroulleteList.table.Remove(key);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void AdmCancelGame(Dictionary<string, string> queryDic)
        {
            string target = queryDic["${target}"];

            bool ex = false;
            string key = "";
            foreach (KeyValuePair<string, RroulleteObj> entry in _RroulleteList.table)
            {
                if (entry.Value.target.ToLower() == target.ToLower() || entry.Value.name.ToLower() == target.ToLower())
                {
                    ex = true;
                    key = entry.Key;
                    break;
                }
            }
            if (!String.IsNullOrEmpty(key))
            {
                string user = _RroulleteList.table[key].name;

                if (ex)
                {
                    _RroulleteList.table.Remove(key);
                    Send(user + " an admin just canceled and deleted your game.");
                }
            }
        }

        public void Send(string ms)
        {
            _cm._twitch.SendMessage(_cm._settings.targetTwitch, ms);
        }

        public string Result(OnWhisperReceivedArgs e, Command c)
        {
            return "";
        }
    }
}

