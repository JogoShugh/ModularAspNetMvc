using System.Web.Mvc;
using FieldReporting.Infrastructure.Messaging;

namespace FieldReporting.Infrastructure.Ui.Mvc.Controllers
{
    public class DefaultController : Controller
    {
        public TResponseType ProcessCommand<TResponseType>(ICommand command) where TResponseType : class
        {
            return MessageProcessor.Instance.Execute<TResponseType>(command);
        }

        public TResponseType ProcessQuery<TResponseType>(IQuery query) where TResponseType : class
        {
            return MessageProcessor.Instance.Execute<TResponseType>(query);
        }
    }
}
