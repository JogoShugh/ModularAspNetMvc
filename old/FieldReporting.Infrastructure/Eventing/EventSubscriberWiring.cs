using FieldReporting.Infrastructure.Composition;

namespace FieldReporting.Infrastructure.Eventing
{
    public class EventSubscriberWiring
    {
        public void WireEventListeners(EventAggregator eventAggregator)
        {
            new PartsList<IRegisterEventSubscribers>(
                registration => registration.RegisterEventListeneresInEventAggregator(eventAggregator));
        }
    }
}
