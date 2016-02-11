using Core.Infrastructure.Messaging;

namespace Core.Modules.UserProfile.Messages.Queries
{
    public class UserProfileEdit : IQuery
    {
        public string UserName { get; set; }
    }
}
