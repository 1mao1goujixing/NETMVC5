using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.Data.Entity;
using TD.IRepository;
using TD.IService;
using TD.Repository;

namespace TD.Service
{
    public class Test: ServiceBase,ITest
    {
       

        TEntity ITest.GetObject<TEntity>(string sql)
        {
            return this.CreateRepository<testRepository>().GetObject<TEntity>(sql);
        }

    }
}
