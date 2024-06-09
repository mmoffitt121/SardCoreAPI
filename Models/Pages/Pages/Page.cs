namespace SardCoreAPI.Models.Pages.Pages
{
    public class Page
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public PageRoot Root { get; set; }
    }
}
