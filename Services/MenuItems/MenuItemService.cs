using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.MenuItems;
using SardCoreAPI.Models.Pages.MenuItems;
using SardCoreAPI.Models.Settings;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Security;
using SardCoreAPI.Services.WorldContext;
using SardCoreAPI.Utility.DataAccess;
using System.Text.Json;

namespace SardCoreAPI.Services.MenuItems
{
    public interface IMenuItemService
    {
        public Task<List<MenuItem>> GetMenuItems();
        public Task<List<ConfigurableMenuItem>> GetConfigurableMenuItems();
        public Task PutConfigurableMenuItems(List<ConfigurableMenuItem> items);
    }

    public class MenuItemService : IMenuItemService
    {
        private IWorldInfoService _worldInfoService;
        private ISecurityService _securityService;
        private IDataService data;

        private static readonly string MENU_ITEM_SETTING = "libratlas.menuItems.";

        public MenuItemService(IWorldInfoService worldInfoService, ISecurityService securityService, IDataService data) 
        {
            _worldInfoService = worldInfoService;
            _securityService = securityService;
            this.data = data;
        }
        public async Task<List<MenuItem>> GetMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            if (_worldInfoService.IsInWorld())
            {
                List<ConfigurableMenuItem> cItems = await GetConfigurableMenuItems();
                menuItems.AddRange(cItems.Select(mi => new MenuItem(mi)));
                menuItems.Add(MenuItemServiceConstants.WORLD_SETUP);
                await _securityService.HasAccess("Library");
            }
            else
            {
                menuItems.Add(MenuItemServiceConstants.WORLDS);
            }
            menuItems.Add(MenuItemServiceConstants.GLOBAL);
            if (!_securityService.IsLoggedIn())
            {
                menuItems.Add(MenuItemServiceConstants.LOGGED_OUT);
            }

            menuItems = await FilterMenuItems(menuItems);

            return menuItems;
        }

        public async Task<List<ConfigurableMenuItem>> GetConfigurableMenuItems()
        {
            SettingJSON? setting = await data.Context.SettingJSON.Where(s => s.Id.Equals(MENU_ITEM_SETTING)).FirstOrDefaultAsync();

            if (setting == null)
            {
                return new List<ConfigurableMenuItem>();
            }

            return JsonSerializer.Deserialize<MenuItemsWrapper>(setting.Setting)?.Items;
        }

        public async Task PutConfigurableMenuItems(List<ConfigurableMenuItem> items)
        {
            data.Context.SettingJSON.Put(new SettingJSON(MENU_ITEM_SETTING, JsonSerializer.Serialize(new MenuItemsWrapper(items))), sj => sj.Id.Equals(MENU_ITEM_SETTING));
            await data.Context.SaveChangesAsync();
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

        private class MenuItemsWrapper
        {
            public List<ConfigurableMenuItem> Items { get; set; }

            public MenuItemsWrapper() { }

            public MenuItemsWrapper(List<ConfigurableMenuItem> items)
            {
                this.Items = items;
            }
        }
    }
}
