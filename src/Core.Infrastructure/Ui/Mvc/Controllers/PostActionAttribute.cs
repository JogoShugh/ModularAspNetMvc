using System.Reflection;
using System.Web.Mvc;

namespace Core.Infrastructure.Ui.Mvc.Controllers
{
    public class PostActionAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(
            ControllerContext controllerContext,
            string actionName,
            MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request[Prefix + methodInfo.Name] != null
                   && !controllerContext.IsChildAction;
        }

        public string Prefix = "Action-";
    }
}
