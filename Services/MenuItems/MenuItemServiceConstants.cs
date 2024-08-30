using SardCoreAPI.Models.MenuItems;
using SardCoreAPI.Services.Security;
using SardCoreAPI.Services.Settings;

namespace SardCoreAPI.Services.MenuItems
{
    public static class MenuItemServiceConstants
    {
        public static readonly string MENU_ITEMS_SETTING = SettingConstants.SETTING_ROOT + ".menu";
        public static readonly string MENU_ITEM_RESOURCE = SecurityServiceConstants.PERMISSION_ROOT + ".Menu";
        public static MenuItem NAVIGATION
        {
            get
            {
                return new MenuItem()
                {
                    Name = "Navigation",
                    Expanded = true,
                    Options = new MenuItem[]
                    {
                        new MenuItem() {
                            Name = "Home",
                            Icon = "home",
                            IsRoot = false,
                            Route = "home",
                            Expanded = true,
                            Resource = $"Library.Menu.Home",
                        },
                        new MenuItem() {
                            Name = "Map",
                            Icon = "map",
                            IsRoot = false,
                            Route = "map",
                            Expanded = true,
                            Resource = $"Library.Menu.Map",
                        },
                        new MenuItem() {
                            Name = "Document",
                            Icon = "history_edu",
                            IsRoot = false,
                            Route = "document",
                            Expanded = true,
                            Resource = $"Library.Menu.Document",
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
                    Name = "World Setup",
                    Expanded = false,
                    Resource = "Library.Setup",
                    Options = new MenuItem[]
                    {
                        new MenuItem() {
                            Name = "Appearance",
                            Icon = "palette",
                            IsRoot = false,
                            Expanded = false,
                            Options = new MenuItem[]
                            {
                                new MenuItem()
                                {
                                    Name = "Themes",
                                    Icon = "palette",
                                    IsRoot = false,
                                    Route = "theme",
                                    Resource = "Library.Setup.Settings"
                                }
                            }
                        },
                        new MenuItem() {
                            Name = "Documents",
                            Icon = "history_edu",
                            IsRoot = false,
                            Expanded = false,
                            Options = new MenuItem[]
                            {
                                new MenuItem()
                                {
                                    Name = "Calendars",
                                    Icon = "calendar_month",
                                    IsRoot = false,
                                    Route = "calendar",
                                    Resource = "Library.Setup.Calendars"
                                },
                                new MenuItem()
                                {
                                    Name = "Document Types",
                                    Icon = "receipt_long",
                                    IsRoot = false,
                                    Route = "document-type",
                                    Resource = "Library.Setup.Types"
                                },
                                new MenuItem()
                                {
                                    Name = "Units",
                                    Icon = "design_services",
                                    IsRoot = false,
                                    Route = "units",
                                    Resource = "Library.Setup.Units"
                                },
                            }
                        },
                        new MenuItem() {
                            Name = "Pages",
                            Icon = "web",
                            IsRoot = false,
                            Expanded = false,
                            Resource = "Library.Setup.Pages",
                            Options = new MenuItem[]
                            {
                                new MenuItem()
                                {
                                    Name = "Menus",
                                    Icon = "menu",
                                    IsRoot = false,
                                    Route = "menus",
                                    Resource = "Library.Setup.Pages"
                                },
                                new MenuItem()
                                {
                                    Name = "Pages",
                                    Icon = "web",
                                    IsRoot = false,
                                    Route = "pages",
                                    Resource = "Library.Setup.Pages"
                                },
                                new MenuItem()
                                {
                                    Name = "Views",
                                    Icon = "table_chart",
                                    IsRoot = false,
                                    Route = "views",
                                    Resource = "Library.Setup.Pages"
                                },
                            }
                        },
                       
                        new MenuItem() {
                            Name = "Security",
                            Icon = "lock_open",
                            IsRoot = false,
                            Expanded = false,
                            Resource = "Library.Setup.Security",
                            Options = new MenuItem[]
                            {
                                new MenuItem()
                                {
                                    Name = "Roles",
                                    Icon = "badge",
                                    IsRoot = false,
                                    Route = "roles",
                                    Resource = "Library.Setup.Security"
                                },
                                new MenuItem()
                                {
                                    Name = "Users",
                                    Icon = "group",
                                    IsRoot = false,
                                    Route = "users",
                                    Resource = "Library.Setup.Security"
                                },
                            }
                        },
                        new MenuItem() {
                            Name = "Storage",
                            Icon = "inventory_2",
                            IsRoot = false,
                            Expanded = false,
                            Options = new MenuItem[]
                            {
                                new MenuItem()
                                {
                                    Name = "Images",
                                    Icon = "image",
                                    IsRoot = false,
                                    Route = "images",
                                    Resource = "Library.Setup.Images"
                                },
                                new MenuItem()
                                {
                                    Name = "Tasks",
                                    Icon = "checklist_rtl",
                                    IsRoot = false,
                                    Route = "tasks",
                                    Resource = "Library.Setup"
                                },
                                /*new MenuItem()
                                {
                                    Name = "Usage",
                                    Icon = "data_usage",
                                    IsRoot = false,
                                    Route = "usage",
                                    Resource = "Library.Setup"
                                },*/
                            }
                        },
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
                    Name = "Global",
                    Expanded = true,
                    Options = new MenuItem[]
                    {
                        new MenuItem() {
                            Name = "Administration",
                            Icon = "shield_person",
                            IsRoot = true,
                            Route = "administration",
                            Resource = "Administrator"
                        },
                        new MenuItem() {
                            Name = "User Settings",
                            Icon = "person",
                            IsRoot = true,
                            Route = "user-settings",
                            Resource = "Viewer",
                        },
                        new MenuItem() {
                            Name = "Library Home",
                            Icon = "home",
                            IsRoot = true,
                            Route = "home",
                        },
                    }
                };
            }
        }
        public static MenuItem WORLDS
        {
            get
            {
                return new MenuItem()
                {
                    Name = "Worlds",
                    Expanded = true,
                    Options = new MenuItem[]
                    {
                        new MenuItem() {
                            Name = "World Browser",
                            Icon = "travel_explore",
                            IsRoot = true,
                            Route = "world-browser",
                        },
                        new MenuItem() {
                            Name = "World Manager",
                            Icon = "construction",
                            IsRoot = true,
                            Route = "world-manager",
                            Resource = "Administrator",
                        },
                    }
                };
            }
        }
        public static MenuItem LOGGED_OUT
        {
            get
            {
                return new MenuItem()
                {
                    Name = "User",
                    Expanded = true,
                    Options = new MenuItem[]
                    {
                        new MenuItem() {
                            Name = "Log In",
                            Icon = "login",
                            IsRoot = true,
                            Route = "login",
                        },
                        new MenuItem() {
                            Name = "Sign Up",
                            Icon = "person_add",
                            IsRoot = true,
                            Route = "register",
                        },
                    }
                };
            }
        }
    }
}
