namespace Core.Infrastructure.Messaging
{
    public abstract class DefaultCommandHandler<TCommandType, TResponseType> :
        IHandleMessage<TCommandType, TResponseType>
        where TCommandType : ICommand
        where TResponseType : class, IMessageResponse, new()
    {
        protected abstract TResponseType Handle(TCommandType command, TResponseType response);

        protected virtual TResponseType CreateResponse()
        {
            return new TResponseType();
        }

        public TResponseType Handle(TCommandType message)
        {
            var response = CreateResponse();

            return Handle(message, response);
        }
    }
}