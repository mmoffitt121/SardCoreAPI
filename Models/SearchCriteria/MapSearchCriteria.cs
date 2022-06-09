namespace SardCoreAPI.Models.SearchCriteria
{
    public class MapSearchCriteria
    {
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

        public string? MapNameSearch { get { return Contains(MapName); } }
        public string? MapDateSearch { get { return Contains(MapDate); } }
        public string? MapAuthorCodeSearch { get { return Contains(MapAuthorCode); } }
        public string? AuthorFirstNameSearch { get { return Contains(AuthorFirstName); } }
        public string? AuthorMiddleNameSearch { get { return Contains(AuthorMiddleName); } }
        public string? AuthorLastNameSearch { get { return Contains(AuthorLastName); } }
        public string? MapPublisherCodeSearch { get { return Contains(MapPublisherCode); } }
        public string? PublisherNameSearch { get { return Contains(PublisherName); } }
        public string? PublisherLocationCodeSearch { get { return Contains(PublisherLocationCode); } }
        public string? LocationNameSearch { get { return Contains(LocationName); } }
        public string? LocationJurisdictionIDSearch { get { return Contains(LocationJurisdictionID); } }
        public string? JurisdictionNameSearch { get { return Contains(JurisdictionName); } }
        public string? ParentJurisdictionIDSearch { get { return Contains(ParentJurisdictionID); } }
        public string? ParentJurisdictionNameSearch { get { return Contains(ParentJurisdictionName); } }
        public string? MapLinkSearch { get { return Contains(MapLink); } }
        public string? MapThumbnailLinkSearch { get { return Contains(MapThumbnailLink); } }
        public string? MapDescriptionSearch { get { return Contains(MapDescription); } }

        private string? StartsWith(string? input)
        {
            if (input == null) return "%%";
            return input + "%";
        }
        private string? EndsWith(string? input)
        {
            if (input == null) return "%%";
            return "%" + input;
        }
        private string? Contains(string? input)
        {
            if (input == null) return "%%";
            return "%" + input + "%";
        }
        private string? Equals(string? input)
        {
            if (input == null) return "%%";
            return input;
        }
    }
}
