using SardCoreAPI.Models.Pages.Pages;

namespace SardCoreAPI.Services.Pages
{
    public class PageServiceConstants
    {
        public static Dictionary<ObjectType, List<ElementSetting>> PAGE_OBJECT_SETTINGS = new Dictionary<ObjectType, List<ElementSetting>>()
        {
            { 
                ObjectType.Root, new List<ElementSetting>() 
                {
                    new ElementSetting(ElementSettingType.String, "Element Name", "Page Root", false),
                } 
            },
            {
                ObjectType.TabGroup, new List <ElementSetting>()
                {
                    new ElementSetting(ElementSettingType.String, "Element Name", "Tab Group", false),
                }
            },
            {
                ObjectType.SplitContainer, new List <ElementSetting>()
                {
                    new ElementSetting(ElementSettingType.String, "Element Name", "Split Container", false),
                    new ElementSetting(ElementSettingType.String, "Orientation", "Vertical", true, new string[] { "Vertical", "Horizontal" }),
                }
            },
            {
                ObjectType.Grid, new List <ElementSetting>()
                {
                    new ElementSetting(ElementSettingType.String, "Element Name", "Grid", false),
                    new ElementSetting(ElementSettingType.Number, "Columns", "2", true),
                }
            },
            {
                ObjectType.View, new List <ElementSetting>()
                {
                    new ElementSetting(ElementSettingType.String, "Element Name", "View", false),
                    new ElementSetting(ElementSettingType.View, "View", "View", true),
                }
            },
        };
    }
}
