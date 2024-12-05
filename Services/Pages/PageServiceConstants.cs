using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Pages.Pages;

namespace SardCoreAPI.Services.Pages
{
    public class PageServiceConstants
    {
        public static Dictionary<ObjectType, List<ElementSetting>> GetPageObjectSettings()
        {
            return new Dictionary<ObjectType, List<ElementSetting>>()
            {
                {
                    ObjectType.Root, new List<ElementSetting>()
                    {
                        new ElementSetting(ElementSettingType.String, "Element Name", "Page Root", false),
                    }
                },
                {
                    ObjectType.Directory, new List<ElementSetting>()
                    {
                        new ElementSetting(ElementSettingType.String, "Element Name", "Directory", false),
                    }
                },
                {
                    ObjectType.TabGroup, new List<ElementSetting>()
                    {
                        new ElementSetting(ElementSettingType.String, "Element Name", "Tab Group", false),
                    }
                },
                {
                    ObjectType.SplitContainer, new List<ElementSetting>()
                    {
                        new ElementSetting(ElementSettingType.String, "Element Name", "Split Container", false),
                        new ElementSetting(ElementSettingType.String, "Orientation", "Vertical", true, new IdNamePair[] { new IdNamePair("Vertical"), new IdNamePair("Horizontal") }),
                        new ElementSetting(ElementSettingType.String, "Split", "Half and Half", true, new IdNamePair[] { new IdNamePair("Half and Half"), new IdNamePair("Left Focus"), new IdNamePair("Right Focus") }),
                    }
                },
                {
                    ObjectType.Document, new List<ElementSetting>()
                    {
                        new ElementSetting(ElementSettingType.String, "Element Name", "Document", false),
                        new ElementSetting(ElementSettingType.Document, "Document", "", false),
                    }
                },
            };
        }
    }
}
