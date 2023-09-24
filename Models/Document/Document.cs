namespace SardCoreAPI.Models.Document
{
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int DocumentTypeId { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
    }
}
