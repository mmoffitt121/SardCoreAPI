using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SardCoreAPI.Utility.Auth
{
    public class WorldAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleStore<IdentityRole> _roleStore;

        public WorldAuthorizeFilter(IHttpContextAccessor httpContextAccessor, IRoleStore<IdentityRole> roleStore)
        {
            _httpContextAccessor = httpContextAccessor;
            _roleStore = roleStore;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!_httpContextAccessor.HttpContext!.Request.Headers["X-Session-Id"].Any())
            {
                context.Result = new UnauthorizedObjectResult(string.Empty);
                return;
            }
        }
    }
}
