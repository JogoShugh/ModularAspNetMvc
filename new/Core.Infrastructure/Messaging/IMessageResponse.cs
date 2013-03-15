namespace Core.Infrastructure.Messaging
{
    public interface IMessageResponse : IMessage
    {
        bool Success { get; set; }
    }
}
