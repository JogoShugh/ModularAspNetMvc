using System.ComponentModel.Composition;
using Core.Infrastructure.Eventing;
using Core.Modules.Authentication.Core.Listeners;
using Core.Modules.Authentication.Messages.Events;
using Core.Modules.UserProfile.Messages.Events;

namespace Core.Modules.UserProfile.Core
{
    [Export(typeof(IRegisterEventSubscribers))]
    public class RegisterEventSubscribers : IRegisterEventSubscribers
    {
        public void RegisterEventListenersInEventAggregator(EventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<LoginFailed>(LoginFailedSubscriber.Receive);
        }
    }
}
