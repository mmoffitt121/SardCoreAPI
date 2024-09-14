using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SardCoreAPI.Services.Security;
using System.Net;

namespace SardCoreAPI.Attributes.Security
{
    public class AuthorizeAction : IAsyncAuthorizationFilter
    {
        private readonly string _resource;
        private readonly bool _strict;
        private readonly ISecurityService _securityService;
        public AuthorizeAction(string resource, bool strict, ISecurityService securityService)
        {
            _resource = resource;
            _strict = strict;
            _securityService = securityService;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (_strict)
            {
                if (!(await _securityService.HasAccess(_resource)))
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                if (!(await _securityService.HasAccessAny(_resource)))
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
