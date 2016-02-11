using System.ComponentModel.Composition;
using Core.Infrastructure.Messaging;
using Core.Modules.Authentication.Core.Services;
using Core.Modules.Authentication.Messages.Commands;

namespace Core.Modules.Authentication.Core
{
    [Export(typeof(IRegisterHandlers))]
    public class RegisterHandlers : IRegisterHandlers
    {
        public void RegisterHandlersInMessageProcessor(MessageProcessor processor)
        {
            processor.RegisterMessageHandler<LoginSubmit, LoginSubmitHandler>();
        }
    }
}
