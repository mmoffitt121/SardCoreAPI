﻿using SardCoreAPI.Attributes.Easy;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SardCoreAPI.Models.Hub.Worlds
{
    [Table("Worlds")]
    public class World
    {
        [Column]
        public int Id { get; set; }
        [Column]
        public string OwnerId { get; set; }
        [Column]
        public string Location { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string? Summary { get; set; }
        [Column]
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

            return errors.Count() > 0 ? string.Join(' ', errors) : null;
        }
    }
}
