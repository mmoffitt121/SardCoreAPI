using SardCoreAPI.Models.Security.LibraryRoles;

namespace SardCoreAPI.Models.Security.Users
{
    public class ViewableLibraryUser : ViewableUser
    {
        public List<Role> LibraryRoles { get; set; }
    }
}
