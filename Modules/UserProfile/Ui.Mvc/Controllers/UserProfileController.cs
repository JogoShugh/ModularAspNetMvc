using System.Web.Mvc;
using FieldReporting.Infrastructure.Messaging;
using FieldReporting.Infrastructure.Ui.Mvc.Controllers;
using FieldReporting.Modules.UserProfile.Messages.Commands;
using FieldReporting.Modules.UserProfile.Messages.Queries;

namespace FieldReporting.Modules.UserProfile.Ui.Mvc.Controllers
{
    public class UserProfileController : DefaultController
    {
        public ActionResult Index(string id)
        {
            var query = new UserProfileEdit {UserName = id};

            var response = ProcessQuery<TypedMessageResponse<string>>(query);

            return Content(response.Data);
        }

        //public ActionResult Index()
        //{
        //    var command = new UserProfileUpdate { Name = "Josh", Intro = "Dev at Lockheed" };

        //    var response = ProcessCommand<TypedMessageResponse<string>>(command);

        //    if (response.Success)
        //    {
        //        return Content(response.Data);
        //    }

        //    return Content("There was an error processing your request...TODO: details");
        //}
    }
}
