namespace FieldReporting.Infrastructure.Eventing
{
    public interface IRegisterEventSubscribers
    {
        void RegisterEventListeneresInEventAggregator(EventAggregator eventAggregator);
    }
}
