using SardCoreAPI.Models.Security.LibraryRoles;
using SardCoreAPI.Services.MenuItems;

namespace SardCoreAPI.Services.Security
{
    public static class SecurityServiceConstants
    {
        public static readonly string DEFAULT_USER_ID = ":NotLoggedIn";

        public static readonly string PERMISSION_ROOT = "Library";
        public static readonly string DOCUMENT_PERMISSION = PERMISSION_ROOT + ".Document";
        public static readonly string ROLE_VIEWER = "Viewer";
        public static readonly string ROLE_SETUP = "Setup";
        public static readonly string ROLE_SECURITY = "Security";
        public static readonly string ROLE_EDITOR = "Editor";
        public static readonly string ROLE_ADMINISTRATOR = "Administrator";

        public static HashSet<string> PERMISSIONS = new HashSet<string> {
            PERMISSION_ROOT,
            PERMISSION_ROOT + ".Administration",
            PERMISSION_ROOT + ".Setup",
            PERMISSION_ROOT + ".Setup.Calendars",
            PERMISSION_ROOT + ".Setup.Pages",
            PERMISSION_ROOT + ".Setup.Security",
            PERMISSION_ROOT + ".Setup.Settings",
            PERMISSION_ROOT + ".Setup.Types",
            PERMISSION_ROOT + ".Setup.Units",
            PERMISSION_ROOT + ".Map",
            PERMISSION_ROOT + ".Location",
            PERMISSION_ROOT + ".General",
            PERMISSION_ROOT + ".Menu",
            PERMISSION_ROOT + ".Menu.Home",
            PERMISSION_ROOT + ".Menu.Map",
            PERMISSION_ROOT + ".Menu.Document",
            MenuItemServiceConstants.MENU_ITEM_RESOURCE,
            DOCUMENT_PERMISSION,
            DOCUMENT_PERMISSION + ".Type"
        };
        public static Role[] DEFAULT_ROLES = new Role[]
        {
            new Role()
            {
                Id = ROLE_VIEWER,
                Permissions = new string[]
                {
                    PERMISSION_ROOT + ".Map.Read",
                    PERMISSION_ROOT + ".Location.Read",
                    DOCUMENT_PERMISSION + ".Read",
                    PERMISSION_ROOT + ".Menu.Home.Read",
                    PERMISSION_ROOT + ".Menu.Map.Read",
                    PERMISSION_ROOT + ".Menu.Document.Read",
                    PERMISSION_ROOT + ".General.Read",
                }
            },
            new Role()
            {
                Id = ROLE_SETUP,
                Permissions = new string[]
                {
                    PERMISSION_ROOT + ".Setup.Calendars",
                    PERMISSION_ROOT + ".Setup.Types",
                    PERMISSION_ROOT + ".Setup.Pages",
                    PERMISSION_ROOT + ".Setup.Settings",
                    PERMISSION_ROOT + ".Setup.Units",
                }
            },
            new Role()
            {
                Id = ROLE_SECURITY,
                Permissions = new string[]
                {
                    PERMISSION_ROOT + ".Setup.Security",
                }
            },
            new Role()
            {
                Id = ROLE_EDITOR,
                Permissions = new string[]
                {
                    PERMISSION_ROOT + ".Map",
                    PERMISSION_ROOT + ".Location",
                    PERMISSION_ROOT + ".Images",
                    DOCUMENT_PERMISSION,
                }
            },
            new Role()
            {
                Id = ROLE_ADMINISTRATOR,
                Permissions = new string[]
                {
                    PERMISSION_ROOT
                }
            }
        };
    }
}
