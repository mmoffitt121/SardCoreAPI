using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SardCoreAPI.Models.Hub.Worlds;

namespace SardCoreAPI.Controllers
{
    public class GenericController : Controller
    {
        public string WorldLocation { 
            get
            {
                //return "new_world";
                StringValues loc;
                if (Request.Headers.TryGetValue("WorldLocation", out loc))
                {
                    return loc.First();
                }
                throw new KeyNotFoundException("WorldLocation");
            }
         }

        public WorldInfo WorldInfo
        {
            get
            {
                return new WorldInfo(WorldLocation);
            }
        }
    }
}
