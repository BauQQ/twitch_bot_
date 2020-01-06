using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using Chatbot.Common.Models;
using Chatbot.Common.Objects;
using Chatbot.Common.Actions;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;
using System.Text.RegularExpressions;


namespace Chatbot.Models.Commands.Entries
{
    [Commandable("math")]
    public class math : ICommandable
    {
        public OnMessageReceivedArgs _e;
        public Command _c;
        public VariableC _variableC = new VariableC();
        public List<ChatterFormatted> _viewers;
        public CommandManager _cm;
        private Query _Query = new Query();

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
            Dictionary<string, string> _queryDic = _Query.extractQuery(c.respons.ToString(), e.ChatMessage.Message.ToString());

            foreach (KeyValuePair<string, string> entry in _queryDic)
            {
                if (entry.Value.ToString() == "error")
                {
                    return "Something is missing";
                }
            }

            List<string> addresses = c.respons.ToString().Split(' ').ToList();
            addresses.RemoveAt(0);

            int result = 0;
            foreach (string s in addresses)
            {
                if (_queryDic.ContainsKey(s))
                {
                    Regex r = new Regex(@"^[0-9]*$");
                    if (r.IsMatch(_queryDic[s]))
                    {
                        if (int.Parse(_queryDic[s]) > 0)
                        {
                            result = result + int.Parse(_queryDic[s]);
                        }
                    }
                    else
                    {
                        return "something is wrong here.";
                    }
                }
            }

            return "The result is " + result;
        }

        public string Result(OnWhisperReceivedArgs e, Command c)
        {
            return "";
        }
    }
}
