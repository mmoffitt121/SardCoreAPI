using SardCoreAPI.Models.MenuItems;
using SardCoreAPI.Services.Security;
using SardCoreAPI.Services.Settings;

namespace SardCoreAPI.Services.MenuItems
{
    public static class MenuItemServiceConstants
    {
        public static readonly string MENU_ITEMS_SETTING = SettingConstants.SETTING_ROOT + ".menu";
        public static readonly string MENU_ITEM_RESOURCE = SecurityServiceConstants.PERMISSION_ROOT + "Menu";
        public static MenuItem NAVIGATION
        {
            get
            {
                return new MenuItem()
                {
                    Name = "Navigation",
                    Expanded = true,
                    Resource = $"{MENU_ITEM_RESOURCE}.Navigation",
                    Options = new MenuItem[]
                    {
                        new MenuItem() {
                            Name = "Home",
                            Icon = "home",
                            IsRoot = false,
                            Route = "home",
                            Expanded = true,
                            Resource = $"{MENU_ITEM_RESOURCE}.Home",
                        },
                    }
                };
            }
        }
        public static MenuItem WORLD_SETUP
        {
            get
            {
                return new MenuItem()
                {
                    Name = "Navigation",
                    Expanded = true,
                    Resource = $"{MENU_ITEM_RESOURCE}.Navigation",
                    Options = new MenuItem[]
                    {

                    }
                };
            }
        }
        public static MenuItem GLOBAL
        {
            get
            {
                return new MenuItem()
                {
                    Name = "Navigation",
                    Expanded = true,
                    Resource = $"{MENU_ITEM_RESOURCE}.Navigation",
                    Options = new MenuItem[]
                    {

                    }
                };
            }
        }
    }
}
