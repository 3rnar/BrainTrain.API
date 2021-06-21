using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Dapper
{
    public abstract class Connect
    {
        public IDbConnection Connection { get; set; }
        protected const string NAME_CONNECTIONSTRING = "cnn";

        public abstract void Open();
        public IDbConnection ChangeDatabase(string _database)
        {
            Connection.ChangeDatabase(_database);
            return Connection;
        }
        public bool Close()
        {
            try
            {
                Connection.Dispose();
                Connection = null;
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Connectionn: " + e);
                return false;
            }
        }
    }
}
