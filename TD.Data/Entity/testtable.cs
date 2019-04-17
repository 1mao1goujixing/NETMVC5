using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.IRepository;

namespace TD.Data.Entity
{
    public class testtable: EntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Pwd { get; set; }

        public string Gender { get; set; }
        public string Home { get; set; }
        public string Remark { get; set; }

    }
}
