using System;

namespace Core.Modules.UserProfile.Messages.Events
{
    public class UserProfileUpdated
    {
        public string UserName { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
