using SardCoreAPI.Attributes.Easy;

namespace SardCoreAPI.Models.Security.LibraryRoles
{
    [Table("RolePermissions")]
    public class RolePermission
    {
        [Column]
        public string RoleId { get; set; }
        [Column]
        public string Permission { get; set; }
    }
}
