using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common.Objects.Viewers
{
    public class LiveViewers
    {
        public Dictionary<string, Viewer> table { get; set; } = new Dictionary<string, Viewer>();
    }
}
