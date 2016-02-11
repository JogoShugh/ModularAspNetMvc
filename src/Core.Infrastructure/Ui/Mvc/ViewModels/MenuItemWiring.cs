using Core.Infrastructure.Composition;

namespace Core.Infrastructure.Messaging
{
    public class MenuItemWiring
    {
        public void WireMenuItems(MessageProcessor processor)
        {
            //new PartsList<IRegisterMenuItems>(
            //    registration => registration.RegisterHandlersInMessageProcessor(processor));
        }
    }
}
