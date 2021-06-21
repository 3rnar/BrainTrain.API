using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

namespace BrainTrain.API.Dapper
{
    public class SqlServer : Connect
    {
        private readonly IConfiguration Configuration;

        public SqlServer()
        {
            Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        }

        public override void Open()
        {
            Connection = new SqlConnection(Configuration.GetConnectionString("Default"));
            Connection.Open();
        }
    }
}
