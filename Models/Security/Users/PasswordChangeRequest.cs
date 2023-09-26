namespace SardCoreAPI.Models.Security.Users
{
    public class PasswordChangeRequest
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
