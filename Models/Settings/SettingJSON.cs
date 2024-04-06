using SardCoreAPI.Attributes.Easy;

namespace SardCoreAPI.Models.Settings
{
    [Table("SettingJSON")]
    public class SettingJSON
    {
        [Column(PrimaryKey = true)]
        public string Id { get; set; }
        [Column]
        public string Setting { get; set; }

        public SettingJSON() { }
        public SettingJSON(string id, string setting)
        {
            Id = id;
            Setting = setting;
        }
    }
}
