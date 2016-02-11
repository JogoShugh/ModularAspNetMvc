using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EntityFramework.Patterns;
using Core.Modules.Authentication.Core.Models;

namespace Core.Modules.Authentication.Core.Repositories
{
    public class UserCredentialRepository : Repository<UserCredential>, IUserCredentialRepository
    {
        public UserCredentialRepository(IObjectSetFactory objectSetFactory) 
            : base(objectSetFactory)
        {
        }

        public bool Authenticate(string userName, string password)
        {
            return Find(x => 
                        x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                        &&
                        x.Password.Equals(password, StringComparison.OrdinalIgnoreCase)).Any();
        }
    }
}
