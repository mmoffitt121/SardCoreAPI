using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Areas.Identity.Data;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.DataAccess.Hub.Worlds;
using SardCoreAPI.DataAccess.Security.LibraryRoles;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security.LibraryRoles;
using SardCoreAPI.Models.Security.Users;
using SardCoreAPI.Models.Settings;
using SardCoreAPI.Services.Easy;
using SardCoreAPI.Services.MenuItems;
using SardCoreAPI.Services.WorldContext;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace SardCoreAPI.Services.Security
{
    public interface ISecurityService
    {
        public Task<bool> HasAccess(string resource);
        public Task<bool> HasAccessAny(string resource);
        public Task<bool> HasGlobalAccess(string resource);
        public Task<Permission> GetAllPermissions(WorldInfo info);
        public Task<Permission> BuildPermissionObject(HashSet<string> permissions);
        public Task<List<Role>> GetRoles(string? roleId, WorldInfo info);
        public Task UpdateRoles(Role[] roles, WorldInfo info);
        public Task DeleteRole(string roleId, WorldInfo info);
        public Task InitializeWorldsWithDefaultRoles();
        public Task<ViewableLibraryUser[]> GetUsersWithRoles(WorldInfo info, string? userId = null);
        public Task UpdateUserRoles(string userId, string[] roles, WorldInfo info);
    }

    public class SecurityService : ISecurityService
    {
        private IEasyDataAccess dataAccess;
        private readonly UserManager<SardCoreAPIUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWorldInfoService _worldInfoService;

        private HashSet<string>? _currentUserPermissions = null;

        #region Access
        public async Task<bool> HasAccess(string resource)
        {
            if (string.IsNullOrEmpty(resource))
            {
                return true;
            }

            string[] path = resource.Split(".");
            bool read = path.Last().Equals("Read");

            HashSet<string> currentUserPermissions = await GetUserPermissions();

            string resourceIterator = "";
            for (int i = 0; i < path.Length; i++)
            {
                resourceIterator += path[i];
                if (currentUserPermissions.Contains(resourceIterator) || (read && currentUserPermissions.Contains($"{resourceIterator}.Read")))
                {
                    return true;
                }
                resourceIterator += ".";
            }
            return false;
        }

        public async Task<bool> HasAccessAny(string resource)
        {
            return (await HasAccess(resource)) || (await GetUserPermissions()).Any(x => x.StartsWith(resource) || x.StartsWith(resource.Replace(".Read", "")));
        }

        public async Task<bool> HasGlobalAccess(string role)
        {
            if (string.IsNullOrEmpty(role)) return true;
            var roles = ((ClaimsIdentity)_httpContextAccessor.HttpContext!.User.Identity!).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            return roles != null && roles.Contains(role);
        }

        private async Task<HashSet<string>> GetUserPermissions()
        {
            if (_currentUserPermissions != null) return _currentUserPermissions;

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("Id");
            List<Role> roles;
            if (userId != null)
            {
                roles = (await GetUsersWithRoles(_worldInfoService.GetWorldInfo(), userId)).FirstOrDefault()?.LibraryRoles;
                
            }
            else
            {
                roles = (await GetUsersWithRoles(_worldInfoService.GetWorldInfo(), SecurityServiceConstants.DEFAULT_USER_ID)).FirstOrDefault()?.LibraryRoles;
            }

            if (roles == null)
            {
                _currentUserPermissions = new HashSet<string>();
                return _currentUserPermissions;
            }

            _currentUserPermissions = new HashSet<string>();
            await Task.WhenAll(roles.Select(async role =>
            {
                Role? r = (await GetRoles(role.Id, _worldInfoService.GetWorldInfo())).FirstOrDefault();
                if (r != null && r.Permissions != null)
                {
                    foreach (string p in r.Permissions)
                    {
                        _currentUserPermissions.Add(p);
                    }
                }
            }));

            return _currentUserPermissions;
        }
        #endregion

        #region Permissions
        public async Task<Permission> GetAllPermissions(WorldInfo info)
        {
            HashSet<string> permissionsSet = new HashSet<string>(SecurityServiceConstants.PERMISSIONS);
            await AddDataPointTypePermissions(permissionsSet, info);
            await AddMenuItemPermissions(permissionsSet, info);

            return await BuildPermissionObject(permissionsSet);
        }

        public async Task<Permission> BuildPermissionObject(HashSet<string> permissions)
        {
            Permission permission = new Permission(SecurityServiceConstants.PERMISSION_ROOT);
            permission.Description = SecurityServiceConstants.PERMISSION_ROOT;
            foreach (string permString in permissions)
            {
                Permission selectedPermission = permission;
                int i = 1;
                string[] permissionArray = permString.Split('.');
                while (i < permissionArray.Length)
                {
                    if (!selectedPermission.ChildrenDictionary.ContainsKey(permissionArray[i]))
                    {
                        string[] arrayWithoutParenthesis = permissionArray[i].Split('(', ')');
                        string id = arrayWithoutParenthesis[0];
                        string description = arrayWithoutParenthesis.Length > 1 ? arrayWithoutParenthesis[1] : arrayWithoutParenthesis[0];
                        selectedPermission.ChildrenDictionary.Add(permissionArray[i], new Permission(id, description));
                    }
                    selectedPermission = selectedPermission.ChildrenDictionary[permissionArray[i]];
                    i++;
                }
            }
            return permission;
        }

        private List<RolePermission> BuildRolePermissionList(string roleId, string[] permissions)
        {
            List<RolePermission> list = new List<RolePermission>();
            foreach (string permission in permissions)
            {
                list.Add(new RolePermission() { RoleId = roleId, Permission = permission });
            }
            return list;
        }

        private async Task AddDataPointTypePermissions(HashSet<string> permissions, WorldInfo info)
        {
            List<DataPointType> types = await dataAccess.Get<DataPointType>(new { }, info);

            foreach (DataPointType type in types)
            {
                permissions.Add(SecurityServiceConstants.DOCUMENT_PERMISSION + ".Type" + "." + type.Id + "(" + type.Name + ")");
            }
        }

        private async Task AddMenuItemPermissions(HashSet<string> permissions, WorldInfo info)
        {
            /*SettingJSON types = await dataAccess.First<SettingJSON>(new { Id = MenuItemServiceConstants. }, info);

            foreach (DataPointType type in types)
            {
                permissions.Add(SecurityServiceConstants.DOCUMENT_PERMISSION + ".Type" + "." + type.Id + "(" + type.Id + ")");
            }*/
        }
        #endregion

        #region Roles
        public async Task<List<Role>> GetRoles(string? roleId, WorldInfo info)
        {
            List<Role> roles = await dataAccess.Get<Role>(new { Id = roleId }, info);
            foreach (Role role in roles)
            {
                List<RolePermission> permissions = await dataAccess.Get<RolePermission>(new { RoleId = role.Id }, info);
                List<string> permissionStrings = new List<string>();
                foreach (RolePermission permission in permissions)
                {
                    permissionStrings.Add(permission.Permission);
                }
                role.Permissions = permissionStrings.ToArray();
            }
            return roles;
        }

        public async Task UpdateRoles(Role[] roles, WorldInfo info)
        {
            foreach (Role role in roles)
            {
                await dataAccess.Put(role, info, true);
                await dataAccess.Delete<RolePermission>(new { RoleId = role.Id }, info);
                await dataAccess.Post<RolePermission>(BuildRolePermissionList(role.Id, role.Permissions ?? Array.Empty<string>()), info);
            }
        }

        public async Task DeleteRole(string roleId, WorldInfo info)
        {
            await dataAccess.Delete<RolePermission>(new { RoleId = roleId }, info);
            await dataAccess.Delete<Role>(new { Id = roleId }, info);
        }
        
        public async Task InitializeWorldsWithDefaultRoles()
        {
            List<World> worlds = await dataAccess.Get<World>();
            await Task.WhenAll(worlds.Select(world =>
            {
                return InitializeWorldWithDefaultRoles(new WorldInfo(world.Location));
            }));
        }

        private async Task InitializeWorldWithDefaultRoles(WorldInfo info)
        {
            await UpdateRoles(SecurityServiceConstants.DEFAULT_ROLES, info);
            World world = (await dataAccess.Get<World>(new { location = info.WorldLocation })).First();
            await UpdateUserRoles(world.OwnerId, new string[] { SecurityServiceConstants.ROLE_ADMINISTRATOR }, info);

            // Initialize default user
            var defaultUserRoles = await dataAccess.Get<UserRole>(new { UserId = SecurityServiceConstants.DEFAULT_USER_ID }, info);
            if (defaultUserRoles == null)
            {
                await UpdateUserRoles(SecurityServiceConstants.DEFAULT_USER_ID, new string[] { SecurityServiceConstants.ROLE_VIEWER }, info);
            }
        }
        #endregion

        #region User Roles
        public async Task<ViewableLibraryUser[]> GetUsersWithRoles(WorldInfo info, string? userId = null)
        {
            List<UserRole> userRoles;
            if (userId != null)
            {
                userRoles = await dataAccess.Get<UserRole>(new { userId }, info);
            }
            else
            {
                userRoles = await dataAccess.Get<UserRole>(new { }, info);
            }
            Dictionary<string, ViewableLibraryUser> users = new Dictionary<string, ViewableLibraryUser>();
            foreach (UserRole userRole in userRoles)
            {
                SardCoreAPIUser user = await _userManager.FindByIdAsync(userRole.UserId);
                users.TryAdd(user.Id, new ViewableLibraryUser()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = Array.Empty<string>(),
                    LibraryRoles = new List<Role>()
                });
                users.GetValueOrDefault(user.Id)!.LibraryRoles.Add(new Role() { Id = userRole.RoleId });
            }
            return users.Values.ToArray();
        }

        public async Task UpdateUserRoles(string userId, string[] roles, WorldInfo info)
        {
            // Delete existing roles
            await dataAccess.Delete<UserRole>(new { UserId = userId }, info);

            // Create new roles
            List<UserRole> roleList = new List<UserRole>();
            foreach (string role in roles)
            {
                roleList.Add(new UserRole(userId, role));
            }
            await dataAccess.Post<UserRole>(roleList, info);
        }
        #endregion

        public SecurityService(
            IEasyDataAccess dataAccess,
            UserManager<SardCoreAPIUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IWorldInfoService worldInfoService)
        {
            this._userManager = userManager;
            this.dataAccess = dataAccess;
            this._httpContextAccessor = httpContextAccessor;
            this._worldInfoService = worldInfoService;
        }
    }
}
