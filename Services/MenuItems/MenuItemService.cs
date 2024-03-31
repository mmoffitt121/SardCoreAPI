using Microsoft.CodeAnalysis.CSharp.Syntax;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.MenuItems;
using SardCoreAPI.Services.Security;
using SardCoreAPI.Services.WorldContext;

namespace SardCoreAPI.Services.MenuItems
{
    public interface IMenuItemService
    {
        public Task<List<MenuItem>> GetMenuItems();
        public Task SetMenuItems(List<MenuItem> menuItems);
    }

    public class MenuItemService : IMenuItemService
    {
        private IWorldInfoService _worldInfoService;
        private ISecurityService _securityService;
        public MenuItemService(IWorldInfoService worldInfoService, ISecurityService securityService) 
        {
            _worldInfoService = worldInfoService;
            _securityService = securityService;
        }
        public async Task<List<MenuItem>> GetMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            if (_worldInfoService.IsInWorld())
            {
                menuItems.Add(MenuItemServiceConstants.NAVIGATION);
                menuItems.Add(MenuItemServiceConstants.WORLD_SETUP);
                await _securityService.HasAccess("Library");
            }
            menuItems.Add(MenuItemServiceConstants.GLOBAL);

            menuItems = await FilterMenuItems(menuItems);

            return menuItems;
        }

        private async Task<List<MenuItem>> FilterMenuItems(IEnumerable<MenuItem> items)
        {
            if (items == null || items.Count() == 0)
            {
                return null;
            }

            List<MenuItem> newList = new List<MenuItem>();

            foreach (MenuItem item in items)
            {
                if (!item.IsRoot && (await _securityService.HasAccessAny(item.Resource)))
                {
                    item.Options = (await FilterMenuItems(item.Options))?.ToArray() ?? new MenuItem[0];
                    newList.Add(item);
                }
                else if (item.IsRoot && (await _securityService.HasGlobalAccess(item.Resource)))
                {
                    item.Options = (await FilterMenuItems(item.Options))?.ToArray() ?? new MenuItem[0];
                    newList.Add(item);
                }
            }

            return newList;
        }

        public async Task SetMenuItems(List<MenuItem> menuItems)
        {

        }
    }
}
