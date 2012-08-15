using System.ComponentModel.Composition;
using FieldReporting.Infrastructure.Eventing;
using FieldReporting.Modules.Authentication.Core.Listeners;
using FieldReporting.Modules.UserProfile.Messages.Events;

namespace FieldReporting.Modules.Authentication.Core
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
