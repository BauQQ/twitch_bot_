using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common.Objects.Viewers
{
    public class Viewer
    {
        public string username { get; set; }
        public int level { get; set; }
        public bool subscriber { get; set; }
        public int chatMod { get; set; }
        public Int64 points { get; set; }
        public DateTime activityStamp { get; set; }
        public List<string> twitchAuto { get; set; }
    }
}
