namespace Core.Infrastructure.Messaging
{
    public interface ILogMessages
    {
        void Log(IMessage message);
    }
}
