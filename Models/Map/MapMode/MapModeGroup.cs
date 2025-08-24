namespace SardCoreAPI.Models.Map.MapMode;

public class MapModeGroup
{
    public int Id { get; set; }
    public int MapId { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public int Index { get; set; }
    public List<MapMode> MapModes { get; set; }
}