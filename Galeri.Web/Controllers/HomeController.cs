using System.Web;
using System.Web.Mvc;

namespace Galeri.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult FileUpload()
        {
            HttpPostedFileBase file = HttpContext.Request.Files[0];
            return Json("");
        }
    }
}