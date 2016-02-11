using Core.Modules.UserProfile.Messages.Events;

namespace Core.Modules.Authentication.Core.Listeners
{
    public static class UserProfileUpdatedSubscriber
    {
        public static void Receive(UserProfileUpdated @event)
        {
            var gotIt = @event.UserName;
        }
    }
}
