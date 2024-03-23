namespace SardCoreAPI.Models.Easy
{
    public class QueryOptions
    {
        public string? OrderBy { get; set; }
        public bool? Descending { get; set; }
        public int? PageNumber { get; set; } = 0;
        public int? PageSize { get; set; }
    }
}
