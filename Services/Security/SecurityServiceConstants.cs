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
                    PERMISSION_ROOT + ".Map.Read",
                    PERMISSION_ROOT + ".Location.Read",
                    PERMISSION_ROOT + ".Images.Read",
                    DOCUMENT_PERMISSION + ".Read",
                }
            },
            new Role()
            {
                Id = "Setup",
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
                Id = "Security",
                Permissions = new string[]
                {
                    PERMISSION_ROOT + ".Setup.Security",
                }
            },
            new Role()
            {
                Id = "Editor",
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
                Id = "Administrator",
                Permissions = new string[]
                {
                    PERMISSION_ROOT
                }
            }
        };
    }
}
