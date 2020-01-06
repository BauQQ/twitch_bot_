using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MySql.Data.Common;
using MySql.Data.MySqlClient;
using MySql.Web;
using System.Diagnostics;
using Chatbot.Common.Objects;
using Chatbot.Common.Actions;

namespace Chatbot.Common.Models
{
    public class SqlManager
    {
        private Settings _setting = new Settings();
        private MySqlConnection _conn;
        private MySqlDataAdapter _adapter = new MySqlDataAdapter();
        private MySqlCommandBuilder _cmdb;
        private DataSet _dataSet;
        private DataTable _dataTable;
        private int _lastViewerID;

        public SqlManager()
        {
            string dbConnString = String.Format("Server={0};Uid={1};Pwd={2};database={3}", 
                _setting.dbHost, 
                _setting.dbUser, 
                _setting.dbPass, 
                _setting.dbName
                );

            _conn = new MySqlConnection(dbConnString);
        }

        public void CRUD(string query, int crud, string table = null, Dictionary<string, string> dic = null)
        {
            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }

            switch (crud)
            {
                case 1:
                    Create(query, dic);
                    break;
                case 2:
                    Read(query);
                    break;
                case 3:
                    Update(query, table);
                    break;
                case 4:
                    Delete(query);
                    break;
                default:
                    break;
            }
        }

        public void Create(string query, Dictionary<string, string> dic = null)
        {
            try
            {
                using (MySqlCommand command = new MySqlCommand(query, _conn))
                {

                    if (dic!=null)
                    {
                        foreach (KeyValuePair<string, string> row in dic)
                        {
                            command.Parameters.AddWithValue(row.Key.ToString(), row.Value.ToString());
                        }
                    }

                    int result = command.ExecuteNonQuery();
                    if (result < 0)
                    {
                        throw new Exception();
                    }
                    _lastViewerID = (int)command.LastInsertedId;
                }
            }
            catch (MySqlException ex)
            {
                throw;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                    _conn.Dispose();
                }
            }
        }

        public void Read(string query)
        {
            
            try
            {
                _adapter = new MySqlDataAdapter(query, _conn);
                _adapter.SelectCommand.CommandType = CommandType.Text;
                _dataTable = new DataTable();
                _adapter.Fill(_dataTable);
            }
            catch (MySqlException ex)
            {
                throw;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _adapter.Dispose();
                    _conn.Close();
                    _conn.Dispose();
                }
            }
        }

        public void Update(string query, string table)
        {
            try
            {
                _adapter.SelectCommand = new MySqlCommand(query, _conn);
                _cmdb = new MySqlCommandBuilder(_adapter);

                _adapter.Fill(_dataSet, table);
                _adapter.Update(_dataSet, table);
            }
            catch (MySqlException ex)
            {
                throw;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _adapter.Dispose();
                    _conn.Close();
                    _conn.Dispose();
                }
            }

        }

        public void Delete(string query)
        {
            try
            {
                using (var cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                throw;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                    _conn.Dispose();
                }
            }
        }

        public DataSet GetSet()
        {
            return _dataSet;
        }

        public void SetSet(DataSet dataSet)
        {
            _dataSet = dataSet;
        }

        public DataTable GetTable()
        {
            return _dataTable;
        }

        public void SetTable(DataTable dataTable)
        {
            _dataTable = dataTable;
        }

        public int GetListViewerID()
        {
            return _lastViewerID;
        }

    }
}
