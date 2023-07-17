namespace SardCoreAPI.Models.Security.JWT
{
    public class AuthResponse
    {
        public bool IsAuthSuccessful { get; set; }
        public string? Token { get; set; }
    }
}
