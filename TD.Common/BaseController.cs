using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TD.Common
{
    /// <summary>
    /// 核心控制器,身份登录功能在这个控制器
    /// </summary>
    public class BaseController : Controller
    {
     
        public TService CreateService<TService>()
        {
            return HttpContainerBuilder.Resolve<TService>();
        }

      
       

    }
}
