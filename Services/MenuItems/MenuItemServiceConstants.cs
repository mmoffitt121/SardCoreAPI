using SardCoreAPI.Models.MenuItems;

namespace SardCoreAPI.Services.MenuItems
{
    public static class MenuItemServiceConstants
    {
        public static MenuItem NAVIGATION = new MenuItem()
        {
            Name = "Navigation",
            Expanded = "True",
            Options = new MenuItem[]
            {

            }
        };
        public static MenuItem WORLD_SETUP = new MenuItem()
        {

        };
        public static MenuItem GLOBAL = new MenuItem()
        {

        };
    }
}
