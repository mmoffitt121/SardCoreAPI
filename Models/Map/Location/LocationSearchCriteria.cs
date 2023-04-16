namespace SardCoreAPI.Models.Map.Location
{
    public class LocationSearchCriteria
    {
        public string? Query { get; set; }
        public bool Cities { get; set; }
        public bool Towns { get; set; }
        public bool Villages { get; set; }
        public bool Hamlets { get; set; }
        public bool Fortresses { get; set; }
    }
}
