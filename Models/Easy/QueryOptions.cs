namespace SardCoreAPI.Models.Easy
{
    public class QueryOptions
    {
        public string? OrderBy { get; set; }
        public bool? Descending { get; set; }
        public int? PageNumber { get; set; } = 0;
        public int? PageSize { get; set; }

        public QueryOptions() { }

        // Consumes query options of passed in variable. Useful for converting query objects
        public QueryOptions(QueryOptions queryOptions)
        {
            OrderBy = queryOptions.OrderBy;
            Descending = queryOptions.Descending;
            PageNumber = queryOptions.PageNumber;
            PageSize = queryOptions.PageSize;

            queryOptions.OrderBy = null;
            queryOptions.Descending = null;
            queryOptions.PageNumber = null;
            queryOptions.PageSize = null;
        }
    }
}
