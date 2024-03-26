using SardCoreAPI.Attributes.Easy;

namespace SardCoreAPI.Models.Settings
{
    [Table("SettingJSON")]
    public class SettingJSON
    {
        [Column]
        public string Id { get; set; }
        [Column]
        public string Setting { get; set; }
    }
}
