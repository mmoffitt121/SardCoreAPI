namespace SardCoreAPI.Models.Map.MapTile
{
    public class TileUploadRequest
    {
        public IFormFile Data { get; set; }
        public int Z { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int LayerId { get; set; }
        public string? ReplaceMode { get; set; }
        public bool? ReplaceRoot { get; set; }
    }
}
