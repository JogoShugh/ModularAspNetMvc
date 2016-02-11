using System.ComponentModel.Composition;
using FieldReporting.Infrastructure.Messaging;
using FieldReporting.Modules.UserProfile.Core.Services;
using FieldReporting.Modules.UserProfile.Messages.Commands;
using FieldReporting.Modules.UserProfile.Messages.Queries;

namespace FieldReporting.Modules.UserProfile.Core
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
