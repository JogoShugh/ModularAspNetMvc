using System;

namespace Core.Modules.Authentication.Messages.Events
{
    public class LoginFailed
    {
        public string UserNameSupplied { get; set; }
        public string PasswordSupplied { get; set; }
        public string ResponseText { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
