using Microsoft.AspNetCore.Identity;

namespace SardCoreAPI.Models.Security.Users
{
    public class User : IdentityUser
    {
        public string GetNormalizedName()
        {
            return UserName.ToUpper();
        }
    }
}
