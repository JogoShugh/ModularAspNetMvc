namespace FieldReporting.Infrastructure.Messaging
{
    public class TypedMessageResponse<TDataType> : DefaultMessageResponse
    {
        public TDataType Data { get; set; }
    }
}