using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.IRepository
{
 public interface IDbObject
    {
        /// <summary>
        /// 数据库连接串
        /// </summary>
        string DbConnectionString { get; set; }

        /// <summary>
        /// 数据库连接类型
        /// </summary>
        DbConnectionType DbConnectionType { get; set; }

        /// <summary>
        /// 数据库连接名
        /// </summary>
        string ConnectionStringName { get; set; }

        /// <summary>
        /// 数据库连接驱动名称
        /// </summary>
        string ProviderName { get; set; }

        /// <summary>
        /// 参数占位符
        /// </summary>
        string ParameterKey { get; set; }

        /// <summary>
        /// 数据连库驱动实例
        /// </summary>
        //DbProviderFactory ProviderFactory { get; set; }

        IDbConnection CreateConnection();

        IDbDataParameter CreateParameter(string name, object value);
    }
}
