namespace SardCoreAPI.Models.Security.Users
{
    public class UserPostResponse
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
