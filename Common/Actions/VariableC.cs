using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Chatbot.Common.Actions
{
    public class VariableC
    {
        public bool CheckThis(string e)
        {            
            if (e.Contains("${"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ReplaceThis(string message, string _s)
        {
            string _ms = message;
            List<string> result = new List<string>();
            Regex reg = new Regex("{.+?}", RegexOptions.Compiled);
            Match m = reg.Match(message);
            while (m.Success)
            {
                result.Add(m.ToString().Replace("{", "").Replace("}", ""));
                m = m.NextMatch();
            }

            foreach (string variable in result)
            {
                _ms = _ms.Replace("${" + variable + "}", _s.ToString());
            }           

            return _ms;
        }

    }
}
