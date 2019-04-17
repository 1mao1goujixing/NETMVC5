using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.Common;
using TD.Data.Entity;

namespace TD.IService
{
    public interface IMySqlTest : IServices
    {
       
        List<testtable> GetAllData();
    }
}
