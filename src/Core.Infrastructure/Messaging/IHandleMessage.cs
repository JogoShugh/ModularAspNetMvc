namespace Core.Infrastructure.Messaging
{
    public interface IHandleMessage<in TMessageType, out TResponseType> 
        where TMessageType : IMessage
        where TResponseType : class
    {
        TResponseType Handle(TMessageType message);
    }
}
