using Microsoft.AspNetCore.Identity;
using SardCoreAPI.Models.Security.Users;

namespace SardCoreAPI.DataAccess.Security.Users
{
    public class PasswordValidator : IPasswordValidator<User>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            return IdentityResult.Success;
        }
    }
}
