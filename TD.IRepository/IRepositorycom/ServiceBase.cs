using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.Common;

namespace TD.IRepository
{
    public class ServiceBase
    {
        public TRepository CreateRepository<TRepository>() where TRepository : IRepositorys
        {
            return HttpContainerBuilder.Resolve<TRepository>();
        }
        public static TRepository StaticCreateRepository<TRepository>() where TRepository : IRepositorys
        {
            return HttpContainerBuilder.Resolve<TRepository>();
        }
    }
}
