using System;
using FieldReporting.Infrastructure.Messaging;
using FieldReporting.Infrastructure.Security;
using FieldReporting.Modules.Authentication.Core.Repositories;
using FieldReporting.Modules.Authentication.Messages.Commands;

namespace FieldReporting.Modules.Authentication.Core.Services
{
    public class LoginSubmitHandler : DefaultCommandHandlerWithTypedResponse<LoginSubmit, string>
    {
        private readonly IUserCredentialRepository _userCredentialRepository;

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
                && command.Password.Equals("lockheed101#"))
            {
                var factory = new FieldReportingPrincipalFactory();
                var principal = factory.CreatePrincipal(command.UserName);

                FieldReportingPrincipal.CurrentPrincipal = principal;

                new FieldReportingFormsAuthentication().SetCookieFromPrincipal(principal);

                response.Data = "Thanks for logging in " + command.UserName;
            }
            else
            {
                response.Data = "Invalid username or password";
            }

            return response;
        }
    }
}