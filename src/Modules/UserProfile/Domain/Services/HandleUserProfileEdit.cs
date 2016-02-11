using System;
using Core.Infrastructure.Messaging;
using Core.Infrastructure.Security;
using Core.Modules.UserProfile.Messages.Queries;

namespace Core.Modules.UserProfile.Core.Services
{
    public class HandleUserProfileEdit : IHandleMessage<UserProfileEdit, TypedMessageResponse<string>>
    {
        public TypedMessageResponse<string> Handle(UserProfileEdit message)
        {
            var response = new TypedMessageResponse<string>();

            if (CorePrincipal.CurrentPrincipal.Identity.Name.Equals(message.UserName, 
                StringComparison.OrdinalIgnoreCase))
            {
                response.Data = "Welcome " + CorePrincipal.CurrentPrincipal.Identity.Name;
            }
            else
            {
                response.Data = "You are not authorized for this action";
            }

            return response;
        }
    }
}
