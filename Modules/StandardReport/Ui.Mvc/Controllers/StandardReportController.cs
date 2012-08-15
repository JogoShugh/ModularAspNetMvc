using System.Web.Mvc;

namespace FieldReporting.Modules.StandardReport.Ui.Mvc.Controllers
{
    public class StandardReportController : Controller
    {
        public ActionResult Index()
        {
            return Content("Hello StandardReport");
        }
    }
}
