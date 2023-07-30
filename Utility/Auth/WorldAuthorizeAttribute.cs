using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SardCoreAPI.Utility.Auth
{
    public class WorldAuthorizeAttribute : TypeFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Console.Write("Hi");
        }

        public WorldAuthorizeAttribute(string resource) : base(typeof(WorldAuthorizeFilter)) 
        { 
            Arguments = new string[] { resource };
        }

        public WorldAuthorizeAttribute() : base(typeof(WorldAuthorizeFilter)) { }
    }
}
