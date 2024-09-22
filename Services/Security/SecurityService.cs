using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Database.DBContext;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security;
using SardCoreAPI.Models.Security.LibraryRoles;
using SardCoreAPI.Models.Security.Users;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.WorldContext;
using System.Security.Claims;

namespace SardCoreAPI.Services.Security
{
    public interface ISecurityService
    {
        public bool IsLoggedIn();
        public Task<bool> HasAccess(string resource);
        public Task<bool> HasAccessAny(string resource);
        public Task<bool> HasGlobalAccess(string resource);
        public Task<HashSet<string>> GetUserPermissions();
        public Task<Permission> GetAllPermissions();
        public Task<Permission> BuildPermissionObject(HashSet<string> permissions);
        public Task<List<Role>> GetRoles(string? roleId);
        public Task UpdateRoles(Role[] roles);
        public Task DeleteRole(string roleId);
        public Task InitializeDefaultUsers();
        public Task InitializeWorldsWithDefaultRoles();
        public Task InitializeWorldWithDefaultRoles(WorldInfo info);
        public Task<ViewableLibraryUser[]> GetUsersWithRoles(string? userId = null);
        public Task UpdateUserRoles(string userId, string[] roles);
    }

    public class SecurityService : ISecurityService
    {
        private readonly UserManager<SardCoreAPIUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWorldInfoService _worldInfoService;
        private IDataService _dataService;

        private HashSet<string>? _currentUserPermissions = null;

        public bool IsLoggedIn()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue("Id") != null;
        }

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

        public async Task<HashSet<string>> GetUserPermissions()
        {
            if (_currentUserPermissions != null) return _currentUserPermissions;

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("Id");
            List<Role>? roles;
            if (userId != null)
            {
                roles = (await GetUsersWithRoles(userId)).FirstOrDefault()?.LibraryRoles;
                
            }
            else
            {
                roles = (await GetUsersWithRoles(SecurityServiceConstants.DEFAULT_USER_ID)).FirstOrDefault()?.LibraryRoles;
            }

            if (roles == null)
            {
                _currentUserPermissions = new HashSet<string>();
                return _currentUserPermissions;
            }

            _currentUserPermissions = new HashSet<string>();
            await Task.WhenAll(roles.Select(async role =>
            {
                Role? r = (await GetRoles(role.Id)).FirstOrDefault();
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
        public async Task<Permission> GetAllPermissions()
        {
            HashSet<string> permissionsSet = new HashSet<string>(SecurityServiceConstants.PERMISSIONS);
            await AddDataPointTypePermissions(permissionsSet);
            await AddMenuItemPermissions(permissionsSet);

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

        private async Task AddDataPointTypePermissions(HashSet<string> permissions)
        {
            List<DataPointType> types = _dataService.Context.DataPointType.ToList();

            foreach (DataPointType type in types)
            {
                permissions.Add(SecurityServiceConstants.DOCUMENT_PERMISSION + ".Type" + "." + type.Id + "(" + type.Name + ")");
            }
        }

        private async Task AddMenuItemPermissions(HashSet<string> permissions)
        {
            /*SettingJSON types = await dataAccess.First<SettingJSON>(new { Id = MenuItemServiceConstants. }, info);

            foreach (DataPointType type in types)
            {
                permissions.Add(SecurityServiceConstants.DOCUMENT_PERMISSION + ".Type" + "." + type.Id + "(" + type.Id + ")");
            }*/
        }
        #endregion

        #region Roles
        public async Task<List<Role>> GetRoles(string? roleId)
        {
            List<Role> roles = _dataService.Context.Role.Where(role => roleId == null ? true : role.Id.Equals(roleId)).ToList();
            foreach (Role role in roles)
            {
                List<RolePermission> permissions = _dataService.Context.RolePermission.Where(rp => rp.RoleId.Equals(role.Id)).ToList();
                List<string> permissionStrings = new List<string>();
                foreach (RolePermission permission in permissions)
                {
                    permissionStrings.Add(permission.Permission);
                }
                role.Permissions = permissionStrings.ToArray();
            }
            return roles;
        }

        public async Task UpdateRoles(Role[] roles)
        {
            foreach (Role role in roles)
            {
                if (_dataService.Context.Role.Any(e => e.Id == role.Id))
                {
                    _dataService.Context.Role.Update(role);
                }
                else
                {
                    _dataService.Context.Role.Add(role);
                }

                _dataService.Context.RolePermission.RemoveRange(_dataService.Context.RolePermission.Where(x => x.RoleId == role.Id).ToList());
                _dataService.Context.RolePermission.AddRange(BuildRolePermissionList(role.Id, role.Permissions ?? Array.Empty<string>()));

                _dataService.Context.SaveChanges();
            }
        }

        public async Task DeleteRole(string roleId)
        {
            _dataService.Context.RolePermission.Where(rp => rp.RoleId.Equals(roleId)).ExecuteDelete();
            _dataService.Context.Role.Where(r => r.Id.Equals(roleId)).ExecuteDelete();
        }

        public async Task InitializeDefaultUsers()
        {
            SardCoreAPIUser? user = await _userManager.FindByNameAsync("admin");
            if (user == null)
            {
                SardCoreAPIUser adminUser = new SardCoreAPIUser() { UserName = "admin", Email = "noemail" };
                var result = await _userManager.CreateAsync(adminUser, "admin");
                string[] roles = { "Viewer", "Editor", "Administrator" };
                await _userManager.AddToRolesAsync(adminUser, roles);
            }
        }
        
        public async Task InitializeWorldsWithDefaultRoles()
        {
            List<World> worlds = _dataService.CoreContext.World.ToList();
            foreach (World w in worlds)
            {
                await InitializeWorldWithDefaultRoles(new WorldInfo(w.Location));
            }
        }

        /// <summary>
        /// Applies the default roles to the world at the location specified in the info
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task InitializeWorldWithDefaultRoles(WorldInfo info)
        {
            await _dataService.UsingWorldContext(info, async () => 
            {
                await UpdateRoles(SecurityServiceConstants.DEFAULT_ROLES);
                await UpdateUserRoles(_dataService.CoreContext.World.Single(w => w.Location.Equals(info.WorldLocation)).OwnerId, new string[] { SecurityServiceConstants.ROLE_ADMINISTRATOR });

                // Initialize default user
                var defaultUserRoles = _dataService.Context.UserRole.Where(ur => ur.UserId.Equals(SecurityServiceConstants.DEFAULT_USER_ID)).ToList();
                if (defaultUserRoles == null || defaultUserRoles.Count() == 0)
                {
                    await UpdateUserRoles(SecurityServiceConstants.DEFAULT_USER_ID, new string[] { SecurityServiceConstants.ROLE_VIEWER });
                }
            });
        }
        #endregion

        #region User Roles
        public async Task<ViewableLibraryUser[]> GetUsersWithRoles(string? userId = null)
        {
            List<UserRole> userRoles;
            if (userId != null)
            {
                userRoles = _dataService.Context.UserRole.Where(x => x.UserId.Equals(userId)).ToList();
            }
            else
            {
                userRoles = _dataService.Context.UserRole.ToList();
            }
            Dictionary<string, ViewableLibraryUser> users = new Dictionary<string, ViewableLibraryUser>();
            foreach (UserRole userRole in userRoles)
            {
                if (SecurityServiceConstants.DEFAULT_USER_ID.Equals(userRole.UserId))
                {
                    users.TryAdd(SecurityServiceConstants.DEFAULT_USER_ID, new ViewableLibraryUser()
                    {
                        Id = SecurityServiceConstants.DEFAULT_USER_ID,
                        UserName = "(Not Logged In)",
                        Roles = Array.Empty<string>(),
                        LibraryRoles = new List<Role>()
                    });
                    users.GetValueOrDefault(SecurityServiceConstants.DEFAULT_USER_ID)!.LibraryRoles.Add(new Role() { Id = userRole.RoleId });
                }
                else
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
            }
            return users.Values.ToArray();
        }

        public async Task UpdateUserRoles(string userId, string[] roles)
        {
            // Delete existing roles
            _dataService.Context.UserRole.RemoveRange(_dataService.Context.UserRole.Where(x => x.UserId.Equals(userId)).ToList());

            // Create new roles
            List<UserRole> roleList = new List<UserRole>();
            foreach (string role in roles)
            {
                roleList.Add(new UserRole(userId, role));
            }
            _dataService.Context.UserRole.AddRange(roleList);

            _dataService.Context.SaveChanges();
        }
        #endregion

        public SecurityService(
            UserManager<SardCoreAPIUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IWorldInfoService worldInfoService,
            IDataService dataService)
        {
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
            this._worldInfoService = worldInfoService;
            this._dataService = dataService;
        }
    }
}
