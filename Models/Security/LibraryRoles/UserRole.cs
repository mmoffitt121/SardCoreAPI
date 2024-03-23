using AspNet.Identity.MySQL;
using SardCoreAPI.Attributes.Easy;

namespace SardCoreAPI.Models.Security.LibraryRoles
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Column]
        public string UserId { get; set; }
        [Column]
        public string RoleId { get; set; }

        public UserRole() { }
        public UserRole(string userId, string roleId)
        {
            UserId = userId;
            RoleId = roleId;
        } 
    }
}
