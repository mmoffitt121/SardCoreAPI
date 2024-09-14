using LinqKit;
using SardCoreAPI.Models.Easy;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Utility.DataAccess;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SardCoreAPI.Models.Pages.Pages
{
    public class PageSearchCriteria : QueryOptions
    {
        public List<string>? Ids { get; set; }
        public string? Query { get; set; }
        public string? Path { get; set; }

        public ExpressionStarter<Page> GetQuery()
        {
            return GetQuery<Page>()
                .AndIf(Ids != null && Ids.Count() > 0, p => Ids!.Contains(p.Id))
                .AndIf(Query != null, p => p.Name.Contains(Query ?? ""))
                .AndIf(Path != null, p => p.Path.Equals(Path ?? ""))
                .AndIf(true, p => true);
        }
    }
}
