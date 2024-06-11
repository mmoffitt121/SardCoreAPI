
namespace SardCoreAPI.Models.Pages.Pages
{
    public class PageElement
    {
        public ObjectType ObjectType { get; set; }
        public string ObjectSettings { get; set; }
        public List<PageElement>? Children { get; set; }
    }

    public enum ObjectType
    {
        Root = 0,
        TabGroup = 1,
        SplitContainer = 2,
        Grid = 3,
        View = 100,
    }
}
