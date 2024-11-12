using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Pages.Pages
{
    public class ElementSetting
    {
        public ElementSettingType Type { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool Configurable { get; set; }
        public IdNamePair[]? PossibleValues { get; set; }

        public ElementSetting() { }

        public ElementSetting(ElementSettingType type, string key, string value, bool configurable, IdNamePair[]? possibleValues = null)
        {
            Type = type;
            Key = key;
            Value = value;
            Configurable = configurable;
            PossibleValues = possibleValues;
        }
    }

    public enum ElementSettingType
    {
        String = 0,
        Number = 2,
        View = 100,
        Document = 101,
    }
}
