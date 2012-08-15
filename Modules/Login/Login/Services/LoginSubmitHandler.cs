using FieldReporting.Infrastructure.Messaging;
using FieldReporting.Modules.Authentication.Messages.Commands;

namespace FieldReporting.Modules.Authentication.Core.Services
{
    public class LoginSubmitHandler : DefaultCommandHandlerWithTypedResponse<LoginSubmit, string>
    {
        protected override TypedMessageResponse<string> 
            HandleCommand(LoginSubmit command, TypedMessageResponse<string> response)
        {
            response.Data = "Hello there " + command.UserName;

            return response;
        }
    }
}
