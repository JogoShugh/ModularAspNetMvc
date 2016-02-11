using System.Web.Mvc;

namespace Core.Modules.Authentication.Ui.Mvc.Controllers
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
