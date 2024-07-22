using Microsoft.EntityFrameworkCore;

namespace SardCoreAPI.Models.Map.MapLayer
{
    [PrimaryKey("Zoom")]
    public class PersistentZoomLevel
    {
        public int Zoom { get; set; }
        public int MapLayerId { get; set; }
    }
}
