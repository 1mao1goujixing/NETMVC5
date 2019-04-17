using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TD.Common;

namespace TD.IRepository
{
    /// <summary>
    /// 数据仓储基类
    /// </summary>
    public abstract class RepositoryBase<TEntity, TDbObject> : IRepositorys where TEntity:EntityBase where TDbObject : IDbObject
    {
        #region Property 属性

        /// <summary>
        /// 数据访问核心对象
        /// </summary>
        protected EntityContext<TDbObject> EntityContext { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        protected string TableName { get; private set; }

        /// <summary>
        /// EntityFramework表对象
        /// </summary>
        protected DbSet<TEntity> Table
        {
            get { return EntityContext.Set<TEntity>(); }
        }
        /// <summary>
        /// EntityFramework数据库对象
        /// </summary>
        protected Database Database
        {
            get { return EntityContext.Database; }
        }
        #endregion

        #region Constructor 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="context">数据访问核心对象</param>
        protected RepositoryBase(EntityContext<TDbObject> context)
        {
            TableName = typeof(TEntity).Name;
            EntityContext = context;
        }

        #endregion

        #region 基础操作

        #region FindAll 查询单表全部数据

        /// <summary>
        /// 查询单表全部数据
        /// </summary>
        /// <returns>表实体对象集合</returns>
        public List<TEntity> FindAll()
        {
            return Table.ToList();
        }


        #endregion

        #region 得到一条记录
        /// <summary>
        /// 得到一条记录,例如 var obj = GetFirst(t=>t.Id=="id");
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TEntity GetFirst(Expression<Func<TEntity, bool>> expression)
        {
            return Table.AsNoTracking().Where(expression).FirstOrDefault();
        }

        #endregion

        #region 得到列表数据
        /// <summary>
        /// 得到列表数据, 例如 var list = GetList(t=>t.value>1);
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<TEntity> GetList(Func<TEntity, bool> expression)
        {
            return Table.AsNoTracking().Where(expression).ToList();
        }

        /// <summary>
        /// 得到列表数据, 例如 var list = GetList(t=>t.value>1);
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<TEntity> GetListOrder<TOrder>(Func<TEntity, bool> expression, Func<TEntity, TOrder> order)
        {
            return Table.AsNoTracking().Where(expression).OrderBy(order).ToList();
        }

        /// <summary>
        /// 得到列表数据, 例如 var list = GetList(t=>t.value>1);
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<TEntity> GetListOrderDesc<TOrder>(Func<TEntity, bool> expression, Func<TEntity, TOrder> order_desc)
        {
            return Table.AsNoTracking().Where(expression).OrderByDescending(order_desc).ToList();
        }

        #endregion

        #region Get 通过主键返回一条数据

        /// <summary>
        /// 通过主键返回一条数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>表实体对象</returns>
        public virtual TEntity Get(string id)
        {
            TEntity entity = Table.Find(id);
            return entity;
        }


        #endregion

        #region Get 通过传入条件查询单条数据

        /// <summary>
        /// 通过传入条件查询单条数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>表实体对象</returns>
        public TEntity Get(string condition, params IDataParameter[] parameters)
        {
            const string sqlFormat = "SELECT * FROM {0} WHERE ROWNUM <= 1 {1}";
            var sql = string.Format(sqlFormat, TableName, condition);
            return EntityContext.Database.SqlQuery<TEntity>(sql, parameters).FirstOrDefault();
        }

        #endregion

        #region Insert 插入单条数据

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="entity">待插入的实体对象</param>
        /// <returns>受影响记录数</returns>
        public int Insert(TEntity entity)
        {
            //EntityContext.Set<TEntity>().Add(entity);
            // return EntityContext.SaveChanges();
            try
            {
                EntityContext.Set<TEntity>().Add(entity);
                return EntityContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder errors = new StringBuilder();
                IEnumerable<DbEntityValidationResult> validationResult = ex.EntityValidationErrors;
                foreach (DbEntityValidationResult result in validationResult)
                {
                    ICollection<DbValidationError> validationError = result.ValidationErrors;
                    foreach (DbValidationError err in validationError)
                    {
                        errors.Append(err.PropertyName + ":" + err.ErrorMessage + "\r\n");
                    }
                }
                var str = errors.ToString();
                return 0;
            }
        }

        public bool ListInsert(List<TEntity> entitys)
        {
            foreach (var t in entitys)
            {
                EntityContext.Set<TEntity>().Add(t);
            }
            var result = EntityContext.SaveChanges() > 0;
            return result;
        }
        #endregion

        #region Update 更新单条数据

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="entity">待更新的实体对象</param>
        /// <returns>受影响记录数</returns>
        public int Update(TEntity entity)
        {
            if (EntityContext.Entry(entity).State == EntityState.Detached)
            {
                EntityContext.Set<TEntity>().Attach(entity);
                EntityContext.Entry(entity).State = EntityState.Modified;
            }

            if (EntityContext.Entry(entity).State == EntityState.Modified)
            {
                return EntityContext.SaveChanges();
            }

            return 0;
        }
        #endregion

      
        #region Delete 通过主键删除单条数据

        /// <summary>
        /// 通过主键删除单条数据
        /// </summary>
        /// <param name="id">待删除的数据主键</param>
        /// <returns>删除是否成功</returns>
        public int Delete(string id)
        {
            var entity = Get(id);
            Table.Remove(entity);
            return EntityContext.SaveChanges();
        }
        #endregion

        #endregion

        #region ExcuteReader

        public IDataReader ExcuteReader(string sql, params IDataParameter[] parameters)
        {
            IDbConnection conn = EntityContext.DbObject.CreateConnection();
            IDbCommand command = conn.CreateCommand();
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            conn.Open();
            var reader = command.ExecuteReader(CommandBehavior.CloseConnection); //当DataReader对象被释放掉以后，数据库连接会自动关闭
            return reader;
        }

        #endregion

        #region ExecuteCommand

        public int ExecuteCommand(string sql, params IDataParameter[] parameters)
        {
            return EntityContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        #endregion

        #region   ExecuteScalar
        public T ExecuteScalar<T>(string query, params IDataParameter[] parameters)
        {
            T reVal;
            IDbConnection connection = EntityContext.DbObject.CreateConnection();
            IDbCommand command = connection.CreateCommand();
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }
            command.CommandType = CommandType.Text;
            command.CommandText = query;

            connection.Open();
            try
            {
                var o = command.ExecuteScalar();

                reVal = (o == null ? default(T) : (T)o);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                command.Dispose();
            }
            return reVal;
        }

        #endregion

        #region Get 通过传入条件查询单条数据

        /// <summary>
        /// 通过传入条件查询单条数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>表实体对象</returns>
        public T GetBySql<T>(string sql, params IDataParameter[] parameters)
        {
            return EntityContext.Database.SqlQuery<T>(sql, parameters).FirstOrDefault();
        }

        #endregion

        #region Get 通过传入条件查询所有数据

        /// <summary>
        /// 通过传入条件查询所有数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>表实体对象</returns>
        public List<T> GetAllBySql<T>(string sql, params IDataParameter[] parameters)
        {
            return EntityContext.Database.SqlQuery<T>(sql, parameters).ToList();
        }


        #endregion

        #region Count
        public int CountBySql(string sql, params IDataParameter[] parameters)
        {
            return Convert.ToInt32(EntityContext.Database.SqlQuery<decimal>(sql, parameters).FirstOrDefault());
        }

        public int GetCounts(Expression<Func<TEntity, bool>> @where)
        {
            int count = 0;
            count = Table.Where(@where).Count();
            return count;
        }
        #endregion

        #region 释放实体
        /// <summary>
        /// 释放实体
        /// </summary>
        /// <param name="entity"></param>
        public void Detach(TEntity entity)
        {
            var objContext = ((IObjectContextAdapter)EntityContext).ObjectContext;
            objContext.Detach(entity);
        }
        #endregion
   

        /// <summary>
        /// 应用于单表
        /// </summary>
        /// <typeparam name="dynamic"></typeparam>
        /// <param name="gridpage"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<dynamic> PageingGrid<dynamic>(GridPage gridpage, string sql)
        {
            string sql1 = "";
            if (gridpage.ispaging)
                sql1 = @"select * from (select temp.*,rownum rowindex from ( " + sql + " ) temp )" + " where rowindex>" + gridpage.BeginIndex + " and rowindex<" + gridpage.EndIndex;
            else
                sql1 = sql;
            return Database.SqlQuery<dynamic>(sql1).ToList();
        }

        /// <summary>
        /// 通过传入条件查询所有数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>表实体对象</returns>
        public List<TEntity> GetAllByCondition(string condition)
        {
            const string sqlFormat = "SELECT * FROM {0} WHERE 1=1 {1}";
            var sql = string.Format(sqlFormat, TableName, condition);
            return EntityContext.Database.SqlQuery<TEntity>(sql).ToList();
        }

        public object GetByID(string id)
        {
            throw new NotImplementedException();
        }

        #region Get 通过传入条件查询单条数据

        /// <summary>
        /// 通过传入条件查询单条数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>表实体对象</returns>
        public TEntity GetByCondition(string condition)
        {
            const string sqlFormat = "SELECT * FROM {0} WHERE ROWNUM <= 1 {1}";
            var sql = string.Format(sqlFormat, TableName, condition);
            return Database.SqlQuery<TEntity>(sql).FirstOrDefault();
        }

        #endregion

        /// <summary>
        /// 通过条件删除数据
        public bool DeleteByCondition(string condition)
        {
            var sql = string.Format("DELETE {0} WHERE 1=1 {1}", TableName, condition);
            return Database.ExecuteSqlCommand(sql) == 1;
        }
        //hzy
        public bool DeleteByCondition(string TableName, string condition)
        {
            var sql = string.Format("DELETE {0} WHERE 1=1 {1}", TableName, condition);
            return Database.ExecuteSqlCommand(sql) > 0;
        }
        #region Count

        protected int Count(string condition, params DbParameter[] parameters)
        {
            const string sqlFormat = "select count(*) FROM {0} where 1=1 {1}";

            string sql = string.Format(sqlFormat, TableName, condition);

            return Convert.ToInt32(Database.SqlQuery<decimal>(sql, parameters).FirstOrDefault());

        }

        protected int CountBySql(string sql)
        {
            return Convert.ToInt32(Database.SqlQuery<decimal>(sql).FirstOrDefault());
        }
        #endregion

        #region Select

        protected List<TEntity> Select(string condition, string sortRules, params DbParameter[] parameters)
        {
            const string sqlFormat = "SELECT * FROM {0} WHERE 1 = 1 {1} {2}";
            string sql = string.Format(sqlFormat, TableName, condition, sortRules);
            return Database.SqlQuery<TEntity>(sql, parameters).ToList();
        }

        protected List<TEntity> Select(int topNumber, string condition, string sortRules,
                                       params DbParameter[] parameters)
        {
            const string sqlFormat = "SELECT * FROM {0} WHERE ROWNUM <= {1} {2} {3}";
            string sql = string.Format(sqlFormat, TableName, topNumber, condition, sortRules);
            return Database.SqlQuery<TEntity>(sql, parameters).ToList();
        }

        protected List<TEntity> SelectTop(int topNumber, string condition, string sortRules,
                                      params DbParameter[] parameters)
        {
            const string sqlFormat = "select* from (SELECT * FROM {0} WHERE  1 = 1 {1} {2}) where ROWNUM <= {3}";
            string sql = string.Format(sqlFormat, TableName, condition, sortRules, topNumber);
            return Database.SqlQuery<TEntity>(sql, parameters).ToList();
        }

        #endregion
        #region 优化过的分页方法
        public List<dynamic> PageingGrid<dynamic>(GridPage gridpage, string tablename, string whereStr)
        {
            string sql1 = "";
            if (gridpage.ispaging)
                sql1 = string.Format(@"SELECT * FROM 
(SELECT T.*, ROWNUM AS rno FROM {0} T WHERE ROWNUM<{1} {3} 
) WHERE rno>{2}", tablename, gridpage.EndIndex, gridpage.BeginIndex, whereStr);
            else
                sql1 = string.Format("SELECT * FROM {0} T WHERE 1=1 {1}", tablename, whereStr);
            return Database.SqlQuery<dynamic>(sql1).ToList();
        }
        #endregion

    }
}
