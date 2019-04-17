using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using TD.Common;
using TD.Data.Context;

namespace TD.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var sa = Assembly.Load("TD.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            var ra = Assembly.Load("TD.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            HttpContainerBuilder.RegisterDataContext<TestContext, HttpContextLifetimeManager>();
            HttpContainerBuilder.RegisterDataContext<MySqlTestContext, HttpContextLifetimeManager>();
            HttpContainerBuilder.RegisterBusinessService<HttpContextLifetimeManager>(sa, true, "TD.Common.IServices");
            HttpContainerBuilder.RegisterDataRepository<HttpContextLifetimeManager>(ra, false, "TD.Common.IRepositorys");
        }
    }
}
