using System;
using Core.Infrastructure.Eventing;
using Core.Infrastructure.Messaging;
using Core.Infrastructure.Security;
using Core.Modules.Authentication.Core.Repositories;
using Core.Modules.Authentication.Messages.Commands;
using Core.Modules.Authentication.Messages.Events;
using Core.Modules.UserProfile.Messages.Events;

namespace Core.Modules.Authentication.Core.Services
{
    public class LoginSubmitHandler : DefaultCommandHandlerWithTypedResponse<LoginSubmit, string>
    {
        //public LoginSubmitHandler(IUserCredentialRepository userCredentialRepository)
        //{
        //    _userCredentialRepository = userCredentialRepository;
        //}

        protected override TypedMessageResponse<string>
            Handle(LoginSubmit command, TypedMessageResponse<string> response)
        {
            //var authenticated = _userCredentialRepository.Authenticate(command.UserName, command.Password);
            //if (authenticated)
            if (command.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase)
                && command.Password.Equals("admin"))
            {
                var factory = new CorePrincipalFactory();
                var principal = factory.CreatePrincipal(command.UserName);

                CorePrincipal.CurrentPrincipal = principal;

                new CoreFormsAuthentication().SetCookieFromPrincipal(principal);

                response.Data = "Thanks for logging in " + command.UserName;
            }
            else
            {
                string message = "Invalid username or password";

                var loginFailedEvent = new LoginFailed
                {
                    OccurredAt = DateTime.Now,
                    PasswordSupplied = command.Password,
                    UserNameSupplied = command.UserName,
                    ResponseText = message
                };
                EventAggregator.Instance.Publish<LoginFailed>(loginFailedEvent);

                response.Data = message;
            }

            return response;
        }
    }
}