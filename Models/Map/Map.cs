namespace SardCoreAPI.Models.Map
{
    public class Map
    {
        public int MapId { get; set; }
        public string? MapName { get; set; }
        public string? MapDate { get; set; }
        public string? MapAuthorCode { get; set; }
        public string? AuthorFirstName { get; set; }
        public string? AuthorMiddleName { get; set; }
        public string? AuthorLastName { get; set; }
        public string? MapPublisherCode { get; set; }
        public string? PublisherName { get; set; }
        public string? PublisherLocationCode { get; set; }
        public string? LocationName { get; set; }
        public string? LocationJurisdictionID { get; set; }
        public string? JurisdictionName { get; set; }
        public string? ParentJurisdictionID { get; set; }
        public string? ParentJurisdictionName { get; set; }
        public string? MapLink { get; set; }
        public string? MapThumbnailLink { get; set; }
        public string? MapDescription { get; set; }
    }
}
