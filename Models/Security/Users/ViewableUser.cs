using SardCoreAPI.Models.Hub.Worlds;

namespace SardCoreAPI.Models.Security.Users
{
    public class ViewableUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
    }
}
