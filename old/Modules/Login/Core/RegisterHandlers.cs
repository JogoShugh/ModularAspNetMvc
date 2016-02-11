using System.ComponentModel.Composition;
using FieldReporting.Infrastructure.Messaging;
using FieldReporting.Modules.Authentication.Core.Services;
using FieldReporting.Modules.Authentication.Messages.Commands;

namespace FieldReporting.Modules.Authentication.Core
{
    [Export(typeof (IRegisterHandlers))]
    public class RegisterHandlers : IRegisterHandlers
    {
        public void RegisterHandlersInMessageProcessor(MessageProcessor processor)
        {
            processor.RegisterMessageHandler<LoginSubmit, LoginSubmitHandler>();
            //CompositionRoot.RegisterServices();
            //processor.RegisterMessageHandler<LoginSubmit, LoginSubmitHandler>(CompositionRoot.Container);
        }
    }
}