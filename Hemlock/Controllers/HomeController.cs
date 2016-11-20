using System.Web.Mvc;
using Hemlock.DAL;
using System.Diagnostics;

namespace Hemlock.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}