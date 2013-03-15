using Core.Infrastructure.Composition;

namespace Core.Infrastructure.Messaging
{
    public class MessageHandlerWiring
    {
        public void WireMessageHandlers(MessageProcessor processor)
        {
            new PartsList<IRegisterHandlers>(
                registration => registration.RegisterHandlersInMessageProcessor(processor));
        }
    }
}
