using Newtonsoft.Json;

namespace SardCoreAPI.Models.Security
{
    public class SecureResource
    {
        [JsonIgnore]
        public string Resource { get; set; }
    }
}
