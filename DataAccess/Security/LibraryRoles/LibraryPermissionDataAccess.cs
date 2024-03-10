using Dapper;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security.LibraryRoles;
using SardCoreAPI.Models.Settings;

namespace SardCoreAPI.DataAccess.Security.LibraryRoles
{
    public class LibraryPermissionDataAccess : GenericDataAccess
    {
        public async Task<List<Role>> GetRoles(string Id, WorldInfo info)
        {
            string sql = @"SELECT * FROM Roles /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("Id LIKE @Id");

            return await Query<Role>(template.RawSql, new { Id }, info);
        }

        public async Task<int> PostRole(Role data, WorldInfo info)
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

        public async Task<int> DeleteUserRole(string Id, WorldInfo info)
        {
            string sql = @"DELETE FROM UserRoles WHERE RoleId = @RoleId AND UserId = @UserId;";
            return await Execute(sql, new { Id }, info);
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

        public async Task<int> PostRolePermission(RolePermission data, WorldInfo info)
        {
            string sql = @"INSERT INTO RolePermissions (RoleId, Permission) VALUES (@RoleId, @Permission)";

            return await Execute(sql, data, info);
        }

        public async Task<int> DeleteRolePermission(string Id, WorldInfo info)
        {
            string sql = @"DELETE FROM UserRoles WHERE RoleId = @RoleId AND UserId = @UserId;";
            return await Execute(sql, new { Id }, info);
        }
    }
}
