using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers
{
    public class GenericController : Controller
    {
        protected async Task<IActionResult> Handle<T>(Task<T> task)
        {
            try
            {
                return Ok(await task);
            }
            catch (Exception e)
            {
                return e.Handle();
            }
        }

        protected async Task<IActionResult> Handle(Task task)
        {
            try
            {
                await task;
                return Ok();
            }
            catch (Exception e)
            {
                return e.Handle();
            }
        }

        public string WorldLocation { 
            get
            {
                //return "test";
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
