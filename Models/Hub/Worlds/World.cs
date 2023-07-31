using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SardCoreAPI.Models.Hub.Worlds
{
    public class World
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

        public string Validate()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(OwnerId.Trim())) { errors.Add("OwnerId is empty."); }
            if (string.IsNullOrEmpty(Location.Trim())) { errors.Add("Location is empty."); }
            if (string.IsNullOrEmpty(Name.Trim())) { errors.Add("Name is empty."); }

            if (!Regex.IsMatch(Location, "^[a-zA-Z0-9_]*$")) { errors.Add("Location is not alphanumeric, including only letters, numbers, and spaces/underscores."); }

            if (Location.Equals("libraries_of")) { errors.Add("Invalid Location Name"); }
            if (Location.Equals("sys")) { errors.Add("Invalid Location Name"); }
            if (Location.Equals("sakila")) { errors.Add("Invalid Location Name"); }
            if (Location.Equals("world")) { errors.Add("Invalid Location Name"); }

            return errors.Count() > 0 ? string.Join(' ', errors) : null;
        }
    }
}
