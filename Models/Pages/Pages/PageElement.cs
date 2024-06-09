
namespace SardCoreAPI.Models.Pages.Pages
{
    public class PageElement
    {
        public ObjectType ObjectType { get; set; }
        public int ObjectId { get; set; }
        public List<PageElement>? Children { get; set; }
    }

    public enum ObjectType
    {
        View = 0,
        Container = 1,
    }
}
