using FieldReporting.Infrastructure.Messaging;

namespace FieldReporting.Modules.UserProfile.Messages.Queries
{
    public class UserProfileEdit : IQuery
    {
        public string UserName { get; set; }
    }
}
