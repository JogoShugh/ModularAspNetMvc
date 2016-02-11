using FieldReporting.Infrastructure.Messaging;

namespace FieldReporting.Modules.UserProfile.Messages.Commands
{
    public class UserProfileUpdate : ICommand
    {
        public string Name { get; set; }
        public string Intro { get; set; }
    }
}
