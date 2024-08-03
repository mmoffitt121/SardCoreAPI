using Microsoft.Extensions.Primitives;
using SardCoreAPI.Models.Hub.Worlds;
using System.Text.RegularExpressions;

namespace SardCoreAPI.Services.WorldContext
{
    public interface IWorldInfoService
    {
        public WorldInfo GetWorldInfo();
        public bool IsInWorld();
        public string WorldLocation { get; }
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
            string worldLocation = WorldLocation;
            if (string.IsNullOrWhiteSpace(worldLocation))
            {
                return null;
            }
            else
            {
                return new WorldInfo(WorldLocation);
            }
        }

        public bool IsInWorld()
        {
            return !string.IsNullOrEmpty(GetWorldInfo()?.WorldLocation);
        }

        public string WorldLocation
        {
            get
            {
                return "adminadmin";
                StringValues loc = new StringValues();
                bool? worldPresent = _contextAccessor.HttpContext?.Request.Headers.TryGetValue("WorldLocation", out loc);
                if (worldPresent == true)
                {
                    string worldLocation = loc.First()!;
                    CheckWorldLocationValid(worldLocation);
                    return worldLocation;
                }
                return null;
            }
        }

        private void CheckWorldLocationValid(string location)
        {
            if (!Regex.IsMatch(location, "^[a-zA-Z0-9_]*$")) { throw new ArgumentException("Invalid world location"); }

            switch (location)
            {
                case "libraries_of":
                case "sys":
                case "sakila":
                case "world":
                case "login":
                case "register":
                case "user-settings":
                case "world-manager":
                case "home":
                case "worlds":
                case "administration":
                case "community":
                case "forum":
                case "admin":
                case "showcase":
                case "map":
                case "new-map":
                case "map-tiles":
                case "timeline":
                case "document":
                    throw new ArgumentException("Invalid world location");
                default:
                    break;
            }
        }
    }
}
