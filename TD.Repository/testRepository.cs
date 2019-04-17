using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.Data.Context;
using TD.Data.Entity;
using TD.IRepository;
using TD.IRepository.IRepositorycom;

namespace TD.Repository
{
    public class testRepository : RepositoryBase<testtable, SMSqlDbObject>
    {
        public testRepository (TestContext context):base(context) {


        }

        public TEntity GetObject<TEntity>(string sql)
        {
            return GetBySql<TEntity>(sql);
        }
    }
}
