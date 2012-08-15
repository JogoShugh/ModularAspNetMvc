namespace FieldReporting.Infrastructure.Messaging
{
    public abstract class DefaultCommandHandlerWithTypedResponse<TCommandType, TTypedResponseGenericDataType> :
        DefaultCommandHandler<TCommandType, TypedMessageResponse<TTypedResponseGenericDataType>>
        where TCommandType : ICommand
    {
    }
}