using System;
using FieldReporting.Infrastructure.Messaging;
using FieldReporting.Infrastructure.Security;
using FieldReporting.Modules.UserProfile.Messages.Queries;

namespace FieldReporting.Modules.UserProfile.Core.Services
{
    public class HandleUserProfileEdit : IHandleMessage<UserProfileEdit, TypedMessageResponse<string>>
    {
        public TypedMessageResponse<string> Handle(UserProfileEdit message)
        {
            var response = new TypedMessageResponse<string>();

            if (FieldReportingPrincipal.CurrentPrincipal.Identity.Name.Equals(message.UserName, 
                StringComparison.OrdinalIgnoreCase))
            {
                response.Data = "Welcome " + FieldReportingPrincipal.CurrentPrincipal.Identity.Name;
            }
            else
            {
                response.Data = "You are not authorized for this action";
            }

            return response;
        }
    }
}
