using FieldReporting.Modules.UserProfile.Messages.Events;

namespace FieldReporting.Modules.Authentication.Core.Listeners
{
    public static class UserProfileUpdatedSubscriber
    {
        public static void Receive(UserProfileUpdated @event)
        {
            var gotIt = @event.UserName;
        }
    }
}
