using System.Web.Mvc;
using FieldReporting.Infrastructure.Messaging;
using FieldReporting.Infrastructure.Ui.Mvc.Controllers;
using FieldReporting.Modules.Authentication.Messages.Commands;
using FieldReporting.Modules.Authentication.Ui.Mvc.ViewModels;

namespace FieldReporting.Modules.Authentication.Ui.Mvc.Controllers.Login
{
    public class LoginController : DefaultController
    {
        public ActionResult Index()
        {
            var model = new LoginSubmitViewModel();
            model.CommandActions.Add("Login", "Submit");
            model.CommandActions.Add("Cancel", "Index");

            return View(model);
        }

        [HttpPost]
        [PostAction]
        public ActionResult Submit(LoginSubmitViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var command = new LoginSubmit
                                  {
                                      Password = viewModel.Password,
                                      UserName = viewModel.UserName
                                  };
                var response = ProcessCommand<TypedMessageResponse<string>>(command);

                return Content(response.Data);
            }

            return Content("TODO: handle errors properly here.");
        }
    }
}
