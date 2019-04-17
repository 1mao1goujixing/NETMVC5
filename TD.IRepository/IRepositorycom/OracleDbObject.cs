using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.IRepository.IRepositorycom
{
    public class OracleObject : IDbObject
    {
        public string ConnectionStringName { get; set; }

        public DbConnectionType DbConnectionType { get; set; }

        public string ProviderName { get; set; }

        private string dbConnectionString = "";

        public string DbConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(dbConnectionString))
                {
                    dbConnectionString = ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString;
                }

                return dbConnectionString;
            }
            set
            {
                dbConnectionString = value;
            }
        }

        public DbProviderFactory ProviderFactory { get; set; }

        public string ParameterKey { get; set; }

        public OracleObject()
        {
            DbConnectionType = DbConnectionType.Oracle;
            ProviderFactory = null;
            ProviderName = "Oracle.ManagedDataAccess.Client";
            ParameterKey = "?";
        }

        public IDbConnection CreateConnection()
        {
            return new OracleConnection(this.DbConnectionString);
        }

        public IDbDataParameter CreateParameter(string name, object value)
        {
            var parameter = new OracleParameter();
            //if (parameter == null) return null;

            parameter.ParameterName = ParameterKey + name;
            parameter.Value = value;
            return parameter;
        }

   
    }
}
