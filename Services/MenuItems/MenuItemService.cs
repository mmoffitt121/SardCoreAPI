using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.MenuItems;

namespace SardCoreAPI.Services.MenuItems
{
    public interface IMenuItemService
    {
        public Task<List<MenuItem>> GetMenuItems();
        public Task SetMenuItems(List<MenuItem> menuItems);
    }

    public class MenuItemService : IMenuItemService
    {
        public async Task<List<MenuItem>> GetMenuItems()
        {
            return null;
        }

        public async Task SetMenuItems(List<MenuItem> menuItems)
        {

        }
    }
}
