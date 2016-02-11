using System.Web.Mvc;

namespace FieldReporting.Modules.Authentication.Ui.Mvc.Controllers
{
    public class AuthenticationController : Controller
    {
        //
        // GET: /Authentication/

        public ActionResult Index()
        {
            return Redirect("Login");
        }

    }
}
