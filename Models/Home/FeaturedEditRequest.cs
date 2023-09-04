using SardCoreAPI.Models.Hub.Worlds;

namespace SardCoreAPI.Models.Home
{
    public class FeaturedEditRequest
    {
        public string WorldLocation { get; set; }
        public List<Featured> Featured { get; set; }
    }
}
