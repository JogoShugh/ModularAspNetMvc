using System;
using Core.Infrastructure.Eventing;
using Core.Infrastructure.Messaging;
using Core.Modules.UserProfile.Messages.Commands;
using Core.Modules.UserProfile.Messages.Events;

namespace Core.Modules.UserProfile.Core.Services
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
