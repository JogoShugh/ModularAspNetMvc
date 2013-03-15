using Core.Infrastructure.Messaging;

namespace Core.Modules.UserProfile.Messages.Commands
{
    public class UserProfileUpdate : ICommand
    {
        public string Name { get; set; }
        public string Intro { get; set; }
    }
}
