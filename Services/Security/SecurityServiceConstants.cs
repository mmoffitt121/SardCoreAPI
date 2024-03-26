using SardCoreAPI.Models.Security.LibraryRoles;
using SardCoreAPI.Services.MenuItems;

namespace SardCoreAPI.Services.Security
{
    public static class SecurityServiceConstants
    {
        public static string PERMISSION_ROOT = "Library";
        public static string DOCUMENT_PERMISSION = PERMISSION_ROOT + ".Document";
        public static string ROLE_VIEWER = "Viewer";
        public static string ROLE_SETUP = "Setup";
        public static string ROLE_SECURITY = "Security";
        public static string ROLE_EDITOR = "Editor";
        public static string ROLE_ADMINISTRATOR = "Administrator";
        public static HashSet<string> PERMISSIONS = new HashSet<string> {
            PERMISSION_ROOT,
            PERMISSION_ROOT + ".Administration",
            PERMISSION_ROOT + ".Setup",
            PERMISSION_ROOT + ".Setup.Calendars",
            PERMISSION_ROOT + ".Setup.Types",
            PERMISSION_ROOT + ".Setup.Pages",
            PERMISSION_ROOT + ".Setup.Security",
            PERMISSION_ROOT + ".Setup.Themes",
            PERMISSION_ROOT + ".Setup.Units",
            PERMISSION_ROOT + ".Map",
            PERMISSION_ROOT + ".Location",
            PERMISSION_ROOT + ".Images",
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
                    PERMISSION_ROOT + ".Images.Read",
                    DOCUMENT_PERMISSION + ".Read",
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
                    PERMISSION_ROOT + ".Setup.Themes",
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
