namespace Core.Infrastructure.Eventing
{
    public interface IRegisterEventSubscribers
    {
        void RegisterEventListenersInEventAggregator(EventAggregator eventAggregator);
    }
}
