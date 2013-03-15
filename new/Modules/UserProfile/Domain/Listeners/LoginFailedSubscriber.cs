using Core.Modules.Authentication.Messages.Events;
using Core.Modules.UserProfile.Messages.Events;

namespace Core.Modules.Authentication.Core.Listeners
{
    public static class LoginFailedSubscriber
    {
        public static void Receive(LoginFailed @event)
        {
            var message = @event.ResponseText;
            var userNameSupplied = @event.UserNameSupplied;
            var passwordSupplied = @event.PasswordSupplied;
            var occuredAt = @event.OccurredAt;
        }
    }
}
