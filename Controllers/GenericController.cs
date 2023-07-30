using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace SardCoreAPI.Controllers
{
    public class GenericController : Controller
    {
        public string WorldLocation { 
            get
            {
                StringValues loc;
                if (Request.Headers.TryGetValue("WorldLocation", out loc))
                {
                    return loc.First();
                }
                throw new KeyNotFoundException("WorldLocation");
            }
         }
    }
}
