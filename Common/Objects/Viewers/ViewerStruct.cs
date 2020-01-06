using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common.Objects.Viewers
{
    public class ViewerStruct
    {
        public Dictionary<string, Viewer> whitelist { get; set; } = new Dictionary<string, Viewer>();
        public Dictionary<string, Viewer> blacklist { get; set; } = new Dictionary<string, Viewer>();
    }
}
