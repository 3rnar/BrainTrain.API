using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Dapper
{
    public class StoredProcedure<C, T> where C : Connect, new()
    {
        private readonly C connection = new();

        private readonly string functionName = string.Empty;

        public StoredProcedure(string _name)
        {
            functionName = _name;
        }

        public List<T> ExecResult(params FunctionParameter[] _listParameters)
        {
            List<T> list = new();

            DynamicParameters parameters = new();
            if (_listParameters.Any())
            {
                foreach (var item in _listParameters)
                {
                    parameters.Add(item.Name, item.Value);
                }
            }

            connection.Open();

            try
            {
                var registro = SqlMapper.Query<T>(connection.Connection, functionName, parameters, commandType: CommandType.StoredProcedure);
                if (registro.Count() > 1)
                    list = registro.ToList();
                else
                    list.Add(registro.First());
            }
            catch (Exception e)
            {
                //ILog log_ = log4net.LogManager.GetLogger("log4Net");
                //log_.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + "-" + System.Reflection.MethodBase.GetCurrentMethod().ToString() + "-" + e.Message + "-SOURCE: " + e.Source);
            }
            finally
            {
                connection.Close();
            }
            return list;
        }

        public void Exec(params FunctionParameter[] _listParameters)
        {
            DynamicParameters parameters = new();

            if (_listParameters.Any())
            {
                foreach (var item in _listParameters)
                {
                    parameters.Add(item.Name, item.Value);
                }
            }
            connection.Open();
            try
            {
                var registro = connection.Connection.Execute(functionName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch
            {
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
