using SardCoreAPI.Models.Pages.MenuItems;
using SardCoreAPI.Models.Security;

namespace SardCoreAPI.Models.MenuItems
{
    [Serializable]
    public class MenuItem : SecureResource
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsRoot { get; set; }
        public string Route { get; set; }
        public bool Expanded { get; set; }
        public MenuItem[] Options { get; set; }

        public MenuItem() { }

        public MenuItem(ConfigurableMenuItem item)
        {
            Name = item.Name;
            Icon = item.Icon;
            Route = item.Route;
            Expanded = item.Expanded;
            Options = item.Options?.Select(o => new MenuItem(o)).ToArray() ?? new MenuItem[0];
        }
    }
}
