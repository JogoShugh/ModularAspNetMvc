using Core.Infrastructure.Messaging;

namespace Core.Modules.Authentication.Messages.Commands
{
    public class LoginSubmit : ICommand
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
