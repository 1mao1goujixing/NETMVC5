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
    public class MyTest : ServiceBase,IMySqlTest
    {
        public List<testtable> GetAllData()
        {
            return this.CreateRepository<MytestRepository>().GetAllData();
        }

       
    }
}
