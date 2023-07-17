using Microsoft.IdentityModel.Tokens;

namespace SardCoreAPI.Models.Security.JWT
{
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
        public System.DateTime Expiration => IssuedAt.Add(ValidFor);
        public System.DateTime NotBefore { get; set; } = System.DateTime.UtcNow;
        public System.DateTime IssuedAt { get; set; } = System.DateTime.UtcNow;
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(5);
        public Func<Task<string>> JtiGenerator =>
          () => Task.FromResult(Guid.NewGuid().ToString());
        public SigningCredentials SigningCredentials { get; set; }
    }
}
