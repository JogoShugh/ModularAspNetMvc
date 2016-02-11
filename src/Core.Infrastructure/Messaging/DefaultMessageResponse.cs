namespace Core.Infrastructure.Messaging
{
    public class DefaultMessageResponse : IMessageResponse
    {
        public DefaultMessageResponse()
        {
            Success = true;
        }

        public bool Success { get; set; }
    }
}
