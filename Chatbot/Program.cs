using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chatbot.Common;
using Chatbot.Common.Objects;
using Chatbot.Common.Models;
using Chatbot.Common.Actions;

namespace Chatbot
{
    public static class Program
    {
        [MTAThread]
        public static void Main(string[] args)
        {
            Main m = new Main();
            m.Run();
            Console.Read();
        }
    }
}
