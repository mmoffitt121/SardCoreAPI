using SardCoreAPI.Attributes.Easy;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SardCoreAPI.Models.Pages.Pages
{
    [Table("Pages")]
    public class Page
    {
        [Column(PrimaryKey = true)]
        public string Id { get; set; }

        [Column(OrderBy = true)]
        public string Name { get; set; }

        [Column]
        public string Description { get; set; }

        [Column(OrderBy = true)]
        public string Path { get; set; }

        public PageElement Root { get; set; }

        [JsonIgnore]
        [Column]
        public string PageData 
        { 
            get 
            {
                return JsonSerializer.Serialize(Root);
            } 
            set
            {
                Root = JsonSerializer.Deserialize<PageElement>(value) ?? new PageElement();
            }
        }
    }
}
