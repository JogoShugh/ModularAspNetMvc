using Core.Infrastructure.Composition;

namespace Core.Infrastructure.Eventing
{
    public class EventSubscriberWiring
    {
        public void WireEventListeners(EventAggregator eventAggregator)
        {
            new PartsList<IRegisterEventSubscribers>(
                registration => registration.RegisterEventListenersInEventAggregator(eventAggregator));
        }
    }
}
