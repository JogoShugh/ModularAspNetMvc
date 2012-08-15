namespace FieldReporting.Infrastructure.Messaging
{
    public interface IRegisterHandlers
    {
        void RegisterHandlersInMessageProcessor(MessageProcessor processor);
    }
}
