using Microsoft.Extensions.Primitives;
using SardCoreAPI.Models.Hub.Worlds;

namespace SardCoreAPI.Services.WorldContext
{
    public interface IWorldInfoService
    {
        public WorldInfo GetWorldInfo();
    }
    public class WorldInfoService : IWorldInfoService
    {
        private IHttpContextAccessor _contextAccessor;

        public WorldInfoService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public WorldInfo GetWorldInfo()
        {
            //return "test";
            StringValues loc = new StringValues();
            bool? worldPresent = _contextAccessor.HttpContext?.Request.Headers.TryGetValue("WorldLocation", out loc);
            if (worldPresent == true)
            {
                return new WorldInfo(loc.First()!);
            }
            return null;
        }
    }
}
