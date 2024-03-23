using SardCoreAPI.Attributes.Easy;
using System.Xml.Linq;

namespace SardCoreAPI.Models.Security.LibraryRoles
{
    [Table("Roles")]
    public class Role
    {
        [Column]
        public string Id { get; set; }
        public string[]? Permissions { get; set; }
    }
}
