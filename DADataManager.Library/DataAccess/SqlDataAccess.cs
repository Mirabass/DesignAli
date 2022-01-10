using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DADataManager.Library.DataAccess
{
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        private readonly IConfiguration _config;
        private ILogger<SqlDataAccess> _logger;
        private bool isClosed;
        public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger)
        {
            _config = config;
            _logger = logger;
        }

        public void Dispose()
        {
            if (isClosed == false)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                }
            }
        }

        public string GetConnectionString(string name)
        {
            //return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DAData;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            return _config.GetConnectionString(name);
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }

        public object SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                var output = connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
                return output;
            }
        }
    }
}