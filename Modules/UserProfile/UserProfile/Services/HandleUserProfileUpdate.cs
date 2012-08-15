using System;
using FieldReporting.Infrastructure.Eventing;
using FieldReporting.Infrastructure.Messaging;
using FieldReporting.Modules.UserProfile.Messages.Commands;
using FieldReporting.Modules.UserProfile.Messages.Events;

namespace FieldReporting.Modules.UserProfile.Core.Services
{
    public class HandleUserProfileUpdate :
        IHandleMessage<UserProfileUpdate, TypedMessageResponse<string>>
    {
        public TypedMessageResponse<string> Handle(UserProfileUpdate message)
        {
            // Do something
            var summary = message.Name + " " + message.Intro;

            EventAggregator.Instance.Publish(new UserProfileUpdated { UserName = message.Name, OccurredAt = DateTime.Now });

            return new TypedMessageResponse<string> { Data = "Thank you " + summary };
        }
    }
}
