using SardCoreAPI.Models.Security.LibraryRoles;

namespace SardCoreAPI.Services.Security
{
    public static class SecurityServiceConstants
    {
        public static string PERMISSION_ROOT = "Library";
        public static string DOCUMENT_PERMISSION = PERMISSION_ROOT + ".Document";
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
            DOCUMENT_PERMISSION,
            DOCUMENT_PERMISSION + ".Type"
        };
        public static Role[] DEFAULT_ROLES = new Role[]
        {
            new Role()
            {
                Id = "Viewer",
                Permissions = new string[]
                {
                    PERMISSION_ROOT + ".Map.View",
                    PERMISSION_ROOT + ".Location.View",
                    PERMISSION_ROOT + ".Images.View",
                    DOCUMENT_PERMISSION + ".View",
                }
            },
            new Role()
            {
                Id = "Setup",
                Permissions = new string[]
                {
                    PERMISSION_ROOT + ".Setup",
                }
            },
            new Role()
            {
                Id = "All Documents",
                Permissions = new string[]
                {
                    PERMISSION_ROOT + ".Setup",
                }
            },
            new Role()
            {
                Id = "Administrator",
                Permissions = new string[]
                {
                    PERMISSION_ROOT
                }
            }
        };
    }
}
