using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common.Actions
{
    public class Query
    {
        public Dictionary<string, string> _queryDic { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> extractQuery(string c, string e)
        {
            Dictionary<string, string> _cld = new Dictionary<string, string>();
            List<string> cli = c.ToString().Split(' ').ToList();
            List<string> clist = e.ToString().Split(' ').ToList();

            for (int i = 0; i < cli.Count; i++)
            {
                if (clist.ElementAtOrDefault(i) != null)
                {
                    if (_cld.ContainsKey(cli[i]))
                    {
                        _cld.Add(cli[i]+i.ToString(), clist[i]);
                    }
                    else
                    {
                        _cld.Add(cli[i], clist[i]);
                    }
                }
                else
                {
                    if (_cld.ContainsKey(cli[i]))
                    {
                        _cld.Add(cli[i]+i.ToString(), "error");
                    }
                    else
                    {
                        _cld.Add(cli[i], "error");
                    }
                }
            }

            return _cld;
        }


        public Dictionary<string, string> extractQuerySongRequest(string c, string e)
        {
            Dictionary<string, string> _cld = new Dictionary<string, string>();
            List<string> cli = c.ToString().Split(' ').ToList();
            List<string> clist = new List<string>();

            int fsi = e.ToString().IndexOf(' ');
            clist.Add(e.ToString().Substring(0, fsi));
            clist.Add(e.ToString().Substring(fsi + 1));

            for (int i = 0; i < cli.Count; i++)
            {
                if (clist.ElementAtOrDefault(i) != null)
                {
                    if (_cld.ContainsKey(cli[i]))
                    {
                        _cld.Add(cli[i] + i.ToString(), clist[i]);
                    }
                    else
                    {
                        _cld.Add(cli[i], clist[i]);
                    }
                }
                else
                {
                    if (_cld.ContainsKey(cli[i]))
                    {
                        _cld.Add(cli[i] + i.ToString(), "error");
                    }
                    else
                    {
                        _cld.Add(cli[i], "error");
                    }
                }
            }

            return _cld;
        }
    }
}
