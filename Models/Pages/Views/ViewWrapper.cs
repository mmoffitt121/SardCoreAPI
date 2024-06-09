using SardCoreAPI.Attributes.Easy;

namespace SardCoreAPI.Models.Pages.Views
{
    [Table("Views")]
    public class ViewWrapper
    {
        [Column(PrimaryKey = true)]
        public string Id { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string View { get; set; }

        public ViewWrapper() { }
        public ViewWrapper(string id, string name, string view)
        {
            Id = id;
            Name = name;
            View = view;
        }
    }
}
