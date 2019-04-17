using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.IRepository
{
    /// <summary>
    /// 数据访问核心对象
    /// </summary>
    public abstract class EntityContext<TDbObject> : DbContext where TDbObject : IDbObject
    {
        public TDbObject DbObject { get; private set; }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="connectionStringName">数据库连接字符串名称</param>
        protected EntityContext(string connectionStringName)
            : base(connectionStringName)
        {
            this.DbObject = System.Activator.CreateInstance<TDbObject>();
            this.DbObject.ConnectionStringName = connectionStringName;
        }

        /// <summary>
        /// 模型创建事件
        /// </summary>
        /// <param name="modelBuilder">模型构造器</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 资源释放方法（关闭数据库连接）
        /// </summary>
        /// <param name="disposing">是否被标识释放</param>
        protected override void Dispose(bool disposing)
        {
            Database.Connection.Close();
            Database.Connection.Dispose();
            base.Dispose(disposing);
        }

    }
}
