using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Models.MenuItems;

namespace SardCoreAPI.Models.Pages.MenuItems
{
    public class ConfigurableMenuItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
        public bool Expanded { get; set; }
        public List<ConfigurableMenuItem>? Options { get; set; }
        public string? ConfigurableMenuItemRoute { get; set; }
    }
}
