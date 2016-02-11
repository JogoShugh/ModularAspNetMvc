using Core.Infrastructure.Messaging;
using Core.Modules.Authentication.Messages.Commands;

namespace Core.Modules.Authentication.Core.Services
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
