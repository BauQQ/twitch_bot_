using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Runtime.InteropServices;

namespace Chatbot.Common.Models
{
    public class InterCom
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public bool IsOnline()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }
    }
}
