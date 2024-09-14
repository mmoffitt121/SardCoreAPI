using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Attributes.Easy;

namespace SardCoreAPI.Models.Pages.Views
{
    [PrimaryKey("Id")]
    public class ViewWrapper
    {
        public string Id { get; set; }
        public string Name { get; set; }
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
