using Dapper;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security.LibraryRoles;
using SardCoreAPI.Models.Settings;

namespace SardCoreAPI.DataAccess.Security.LibraryRoles
{
    public interface ILibraryPermissionDataAccess
    {
        public Task<List<Role>> GetRoles(string? Id, WorldInfo info);
        public Task<int> PutRole(Role data, WorldInfo info);
        public Task<int> DeleteRole(string Id, WorldInfo info);
        public Task<List<UserRole>> GetUserRoles(string? roleId, string? userId, WorldInfo info);
        public Task<int> PostUserRole(UserRole data, WorldInfo info);
        public Task<int> DeleteUserRole(string RoleId, string UserId, WorldInfo info);
        public Task<List<RolePermission>> GetRolePermissions(string? roleId, string? permission, WorldInfo info);
        public Task<int> PostRolePermissions(List<RolePermission> data, WorldInfo info);
        public Task<int> DeleteRolePermissions(string roleId, WorldInfo info);
    }

    public class LibraryPermissionDataAccess : GenericDataAccess, ILibraryPermissionDataAccess
    {
        public async Task<List<Role>> GetRoles(string? Id, WorldInfo info)
        {
            string sql = @"SELECT * FROM Roles /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (Id != null) builder.Where("Id LIKE @Id");

            return await Query<Role>(template.RawSql, new { Id }, info);
        }

        public async Task<int> PutRole(Role data, WorldInfo info)
        {
            string sql = @"INSERT INTO Roles (Id) VALUES (@Id) ON DUPLICATE KEY UPDATE Id = @Id";

            return await Execute(sql, data, info);
        }

        public async Task<int> DeleteRole(string Id, WorldInfo info)
        {
            string sql = @"DELETE FROM Roles WHERE Id = @Id;";
            return await Execute(sql, new { Id }, info);
        }

        public async Task<List<UserRole>> GetUserRoles(string? roleId, string? userId, WorldInfo info)
        {
            string sql = @"SELECT * FROM UserRoles /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (roleId != null) builder.Where("RoleId LIKE @RoleId");
            if (userId != null) builder.Where("UserId LIKE @UserId");

            return await Query<UserRole>(template.RawSql, new { roleId, userId }, info);
        }

        public async Task<int> PostUserRole(UserRole data, WorldInfo info)
        {
            string sql = @"INSERT INTO UserRoles (RoleId, UserId) VALUES (@RoleId, @UserId)";

            return await Execute(sql, data, info);
        }

        public async Task<int> DeleteUserRole(string UserId, string RoleId, WorldInfo info)
        {
            string sql = @"DELETE FROM UserRoles WHERE RoleId = @RoleId AND UserId = @UserId;";
            return await Execute(sql, new { UserId, RoleId }, info);
        }

        public async Task<List<RolePermission>> GetRolePermissions(string? roleId, string? permission, WorldInfo info)
        {
            string sql = @"SELECT * FROM RolePermissions /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (roleId != null) builder.Where("RoleId LIKE @RoleId");
            if (permission != null) builder.Where("Permission LIKE @Permission");

            return await Query<RolePermission>(template.RawSql, new { roleId, permission }, info);
        }

        public async Task<int> PostRolePermissions(List<RolePermission> data, WorldInfo info)
        {
            string sql = @"INSERT INTO RolePermissions (RoleId, Permission) VALUES (@RoleId, @Permission)";

            return await Execute(sql, data, info);
        }

        public async Task<int> DeleteRolePermissions(string roleId, WorldInfo info)
        {
            string sql = @"DELETE FROM RolePermissions WHERE RoleId = @RoleId;";
            return await Execute(sql, new { roleId }, info);
        }
    }
}
