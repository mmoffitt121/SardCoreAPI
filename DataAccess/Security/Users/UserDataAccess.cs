using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Security.JWT;
using SardCoreAPI.Models.Security.Users;

namespace SardCoreAPI.DataAccess.Security.Users
{
    public class UserDataAccess : GenericDataAccess, IUserStore<User>, IUserPasswordStore<User>
    {
        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            string sql = @"SELECT * FROM Users WHERE UserName = @UserName";
            return (await QueryFirst<User>(sql, user, true))?.UserName ?? null;
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            string sql = @"SELECT * FROM Users WHERE Id = @Id";
            return (await QueryFirst<User>(sql, user, true))?.UserName ?? null;
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            string sql = @"UPDATE Users SET UserName = @UserName WHERE Id = @Id";
            await Execute(sql, new { Id = user.Id, UserName = userName }, true);
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return user.GetNormalizedName();
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            /*string sql = @"UPDATE Users SET UserName = @NormalizedUserName WHERE Id = @Id";
            await Execute(sql, new { Id = user.Id, UserName = normalizedName});*/
            
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            string sql = @"INSERT INTO Users (
                    Id, 
                    UserName, 
                    Email, 
                    EmailConfirmed,
                    PasswordHash, 
                    PhoneNumber, 
                    PhoneNumberConfirmed, 
                    TwoFactorEnabled, 
                    LockoutEndDateUtc, 
                    LockoutEnabled, 
                    AccessFailedCount
                ) VALUES (
                    @Id, 
                    @UserName, 
                    @Email, 
                    @EmailConfirmed,
                    @PasswordHash, 
                    @PhoneNumber, 
                    @PhoneNumberConfirmed, 
                    @TwoFactorEnabled, 
                    @LockoutEnd, 
                    @LockoutEnabled, 
                    @AccessFailedCount)";

            return await Execute(sql, user, true) == 1 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            string sql = @"UPDATE Users SET 
                    UserName = @UserName, 
                    Email = @Email, 
                    EmailConfirmed = @EmailConfirmed,
                    PasswordHash = @PasswordHash, 
                    PhoneNumber = @PhoneNumber, 
                    PhoneNumberConfirmed = @PhoneNumberConfirmed, 
                    TwoFactorEnabled = @TwoFactorEnabled, 
                    LockoutEndDateUtc = @LockoutEnd, 
                    LockoutEnabled = @LockoutEnabled, 
                    AccessFailedCount = @AccessFailedCount
                WHERE Id = @Id";

            return await Execute(sql, user, true) == 1 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            string sql = "DELETE FROM Users WHERE Id LIKE @Id";
            return await Execute(sql, user, true) == 1 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            string sql = "SELECT * FROM Users WHERE Id LIKE @userId;";
            return await QueryFirst<User>(sql, new { userId }, true);
        }

        public async Task<User> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            string sql = "SELECT * FROM Users WHERE UserName LIKE @name;";
            return await QueryFirst<User>(sql, new { name }, true);
        }

        public void Dispose()
        {

        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            string sql = @"UPDATE Users SET PasswordHash = @PasswordHash WHERE Id LIKE @Id";
            int result = await Execute(sql, user, true);
            Console.Write(result);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            string sql = @"SELECT * FROM Users WHERE Id = @Id";
            return (await QueryFirst<User>(sql, user, true))?.PasswordHash ?? null;
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            string sql = @"SELECT * FROM Users WHERE Id = @Id";
            User result = await QueryFirst<User>(sql, user, true);
            if (result == null) return false;
            return result.PasswordHash.IsNullOrEmpty() ? true : false;
        }
    }
}
