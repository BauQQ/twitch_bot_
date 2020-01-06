using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chatbot.Common.Models;
using Chatbot.Common.Objects;
using Chatbot.Common.Actions;
using System.Data;

namespace Chatbot.Models.Mysql
{
    public class MManager
    {
        public SqlManager _sqlManager = new SqlManager();

        public MManager()
        {
            
        }

        public void AddViewer(string username, int lvl, int subscriber)
        {
            _sqlManager.CRUD(String.Format("SELECT * FROM viewers WHERE username='{0}' LIMIT 1", username), 2);
            DataTable dt = _sqlManager.GetTable();
            if (dt == null || dt.Select("username ='"+ username + "'").Length==0)
            {
                    string query = String.Format("INSERT INTO viewers (username, lvl, subscriber) VALUES ('{0}', '{1}', '{2}') ON DUPLICATE KEY UPDATE username=username;",
                      username,
                      lvl,
                      subscriber);
                    _sqlManager.CRUD(query, 1, null);
                    int lastId = _sqlManager.GetListViewerID();                
            }
          
        }

        public void GetCommands()
        {

        }

        public void Test()
        {
            _sqlManager.CRUD("SELECT * FROM viewers AS V1 INNER JOIN viewer_twitchauto AS V2 ON V2.id = V1.id WHERE V1.username = 'berganderf'", 2);
            var v = _sqlManager.GetTable();
            foreach (DataRow dt in v.Rows)
            {
                Console.WriteLine(dt["twitchAuto"].ToString());
            }
        }
    }
}
