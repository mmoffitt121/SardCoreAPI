using SardCoreAPI.Attributes.Easy;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SardCoreAPI.Models.Security.LibraryRoles
{
    //[Table("Roles")]
    public class Role
    {
        //[Column]
        public string Id { get; set; }
        [NotMapped]
        public string[]? Permissions { get; set; }
    }
}
