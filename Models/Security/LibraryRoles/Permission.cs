namespace SardCoreAPI.Models.Security.LibraryRoles
{
    public class Permission
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Dictionary<string, Permission> Children { get; set; } = new Dictionary<string, Permission>();

        public Permission() { }
        public Permission(string id)
        {
            Id = id;
        }
        public Permission(string id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}
