using System.Collections.Generic;
using System.Data.Common;

namespace TD.Common
{
    public interface IRepositorys<TEntity> where TEntity : class
    {
        List<TEntity> GetALL();

        TEntity Get(string Id);

        bool Update(TEntity entity);

        bool Insert(TEntity entity);

        bool Delete(string Id);

        int ExecuteCommand(string sql, params DbParameter[] parameters);
    }


    public interface IRepositorys
    {
    }
}
