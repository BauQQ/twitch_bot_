using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chatbot.Common.Objects
{
    public class Config
    {
        public readonly string SETTING_PATH = $@"{Application.StartupPath}\bin\cfg\settings.cfg";
        public readonly string STORAGE_PATH = $@"{Application.StartupPath}\bin\storage\";
        public readonly string BIN_PATH = $@"{Application.StartupPath}\bin\";
        public readonly string BASE_PATH = $@"{Application.StartupPath}\";
    }
}
