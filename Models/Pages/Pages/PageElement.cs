
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SardCoreAPI.Models.Pages.Pages
{
    public class PageElement
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ObjectType ObjectType { get; set; }
        public string ObjectSettings { get; set; }
        public List<PageElement> Children { get; set; }

        public PageElement() 
        {
            if (Children == null)
            {
                Children = new List<PageElement>();
            }
        }

        public PageElement(ObjectType type)
        {
            ObjectType = type;
            if (Children == null)
            {
                Children = new List<PageElement>();
            }
        }
    }

    public enum ObjectType
    {
        Root = 0,
        TabGroup = 1,
        SplitContainer = 2,
        Directory = 3,
        View = 100,
        Document = 101,
    }
}
