namespace SardCoreAPI.Models.Security.Users
{
    public class PasswordResetRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
