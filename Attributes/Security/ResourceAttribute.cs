using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SardCoreAPI.Attributes.Security
{
    public class ResourceAttribute : TypeFilterAttribute
    {
        public string ResourceName { get; set; }
        public ResourceAttribute(string resourceName, bool strict = true) : base(typeof(AuthorizeAction))
        {
            this.ResourceName = resourceName;
            Arguments = new object[] {
                resourceName,
                strict
            };
        }
    }
}
