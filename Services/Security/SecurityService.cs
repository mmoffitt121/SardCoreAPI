using SardCoreAPI.Areas.Identity.Data;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security.LibraryRoles;

namespace SardCoreAPI.Services.Security
{
    public class SecurityService
    {
        public async Task<Permission> GetAllPermissions(WorldInfo info)
        {
            HashSet<string> permissionsSet = new HashSet<string>(SecurityServiceConstants.PERMISSIONS);
            await AddDataPointTypePermissions(permissionsSet, info);

            return await BuildPermissionObject(permissionsSet);
        }

        public async Task<Permission> BuildPermissionObject(HashSet<string> permissions)
        {
            Permission permission = new Permission(SecurityServiceConstants.PERMISSION_ROOT);
            foreach (string permString in permissions)
            {
                Permission selectedPermission = permission;
                int i = 1;
                string[] permissionArray = permString.Split('.');
                while (i < permissionArray.Length)
                {
                    if (!selectedPermission.Children.ContainsKey(permissionArray[i]))
                    {
                        string[] arrayWithoutParenthesis = permissionArray[i].Split('(', ')');
                        string id = arrayWithoutParenthesis[0];
                        string description = arrayWithoutParenthesis.Length > 1 ? arrayWithoutParenthesis[1] : arrayWithoutParenthesis[0];
                        selectedPermission.Children.Add(permissionArray[i], new Permission(id, description));
                    }
                    selectedPermission = selectedPermission.Children[permissionArray[i]];
                    i++;
                }
            }
            return permission;
        }

        private async Task AddDataPointTypePermissions(HashSet<string> permissions, WorldInfo info)
        {
            List<DataPointType> types = await new DataPointTypeDataAccess().GetDataPointTypes(new DataPointTypeSearchCriteria(), info);

            foreach (DataPointType type in types)
            {
                permissions.Add(SecurityServiceConstants.DOCUMENT_PERMISSION + ".Type" + "." + type.Id + "(" + type.Name + ")");
            }
        }

        public async Task UpdateRole(string role, string[] permissions, WorldInfo info)
        {

        }

        public async Task DeleteRole(WorldInfo info)
        {

        }

        public void CheckAccess(SardCoreAPIUser user, WorldInfo info)
        {
            
        }

        private void CreateDefaultRoles()
        {

        }
    }
}
