using System.ComponentModel.Composition;
using Core.Infrastructure.Messaging;
using Core.Modules.UserProfile.Core.Services;
using Core.Modules.UserProfile.Messages.Commands;
using Core.Modules.UserProfile.Messages.Queries;

namespace Core.Modules.UserProfile.Core
{
    [Export(typeof(IRegisterHandlers))]
    public class RegisterHandlers : IRegisterHandlers
    {
        public void RegisterHandlersInMessageProcessor(MessageProcessor processor)
        {
            processor.RegisterMessageHandler<UserProfileUpdate, HandleUserProfileUpdate>();
            processor.RegisterMessageHandler<UserProfileEdit, HandleUserProfileEdit>();
        }
    }
}
