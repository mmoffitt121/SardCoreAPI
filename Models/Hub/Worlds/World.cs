using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Models.Common;
using System.Text.RegularExpressions;

namespace SardCoreAPI.Models.Hub.Worlds
{
    public class World : IValidatable
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public DateTime? CreatedDate { get; set; }

        public void Normalize()
        {
            Location = Location.Trim().ToLower().Replace(' ', '_');
        }

        public List<string> Validate()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(OwnerId.Trim())) { errors.Add("OwnerId is empty."); }
            if (string.IsNullOrEmpty(Location.Trim())) { errors.Add("Location is empty."); }
            if (string.IsNullOrEmpty(Name.Trim())) { errors.Add("Name is empty."); }

            if (!Regex.IsMatch(Location, "^[a-zA-Z0-9_]*$")) { errors.Add("Location is not alphanumeric, including only letters, numbers, and spaces/underscores."); }

            switch (Location)
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
                    errors.Add("Invalid Location Name");
                    break;
                default:
                    break;
            }

            return errors;
        }
    }
}
