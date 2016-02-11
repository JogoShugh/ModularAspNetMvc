using Core.Infrastructure.Eventing;
using Core.Infrastructure.Messaging;
using Core.Infrastructure.Ui.Mvc.Modularity;

namespace Core.Infrastructure.Boot
{
    public class BootStrapper
    {
        public void Boot()
        {
            var moduleLoader = new ModuleLoader();
            moduleLoader.LoadAllModules();

            var messageHandlerWiring = new MessageHandlerWiring();
            messageHandlerWiring.WireMessageHandlers(MessageProcessor.Instance);

            var eventSubscriberWiring = new EventSubscriberWiring();
            eventSubscriberWiring.WireEventListeners(EventAggregator.Instance);
        }

        public void RegisterComponents()
        {
        }
    }
}
