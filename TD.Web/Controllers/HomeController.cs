using System.Web.Mvc;
using TD.Common;
using TD.Data.Entity;
using TD.IService;

namespace TD.Web.Controllers
{
    public class HomeController : BaseController
    {
        public  string staticstr = string.Empty;
        // GET: Home
        public ActionResult Index()
        {
            
         var result = this.CreateService<ITest>().GetObject<testtable>("select * from testtable");
            staticstr = result.Name;
            //var results = this.CreateService<IMySqlTest>().GetAllData();
            ViewBag.ss = staticstr;
            return View();
        }
        public ActionResult About() {
            staticstr = "1223";
            ViewBag.ss = staticstr;
            return View();
        }
        public ActionResult test()
        {
            ViewBag.ss = staticstr;
            return View();
        }
    }
}