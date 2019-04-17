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
    public class MytestRepository : RepositoryBase<testtable, MySqlDbObject>
    {
        public MytestRepository(MySqlTestContext context):base(context) {


        }

        public  List<testtable> GetAllData() {

            return GetAllBySql<testtable>("select * from testtable");
        }
     
    }
}
