using System.ComponentModel.Composition;
using Core.Infrastructure.Eventing;
using Core.Modules.Authentication.Core.Listeners;
using Core.Modules.UserProfile.Messages.Events;

namespace Core.Modules.Authentication.Core
{
    [Export(typeof(IRegisterEventSubscribers))]
    public class RegisterEventSubscribers : IRegisterEventSubscribers
    {
        public void RegisterEventListeneresInEventAggregator(EventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<UserProfileUpdated>(UserProfileUpdatedSubscriber.Receive);
        }
    }
}
