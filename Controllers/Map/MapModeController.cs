using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Models.Map;
using SardCoreAPI.Models.Map.MapMode;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Controllers.Map;

[ApiController]
[Route("Library/[controller]/[action]")]
public class MapModeController : GenericController
{
    private readonly IDataService data;

    public MapModeController(IDataService data)
    {
        this.data = data;
    }

    #region Map

    [HttpGet]
    [Resource("Library.Map.Read")]
    public async Task<IActionResult> GetMapModes([FromQuery] int MapId)
    {
        return await Handle(data.Context.MapModeGroup
            .Where(mapModeGroup => mapModeGroup.MapId.Equals(MapId))
            .Include(mapModeGroup => mapModeGroup.MapModes)
            .ToListAsync());
    }

    [HttpPut]
    [Resource("Library.Map")]
    public async Task<IActionResult> PutMapModes([FromBody] MapModeGroup mmg)
    {
        return await Handle(_putMapModes(mmg));
    }
    
    private async Task _putMapModes(MapModeGroup mmg)
    {
        MapModeGroup? existing = await data.Context.MapModeGroup.SingleOrDefaultAsync(m => m.Id.Equals(mmg.Id));
        if (existing != null)
        {
            data.Context.MapMode.RemoveRange(existing.MapModes);
            data.Context.MapModeGroup.Remove(existing);
        }
        
        data.Context.MapModeGroup.Add(mmg);
        await data.Context.SaveChangesAsync();
    }

    [HttpDelete]
    [Resource("Library.Map")]
    public async Task<IActionResult> DeleteMapMode([FromQuery] int Id)
    {
        return await Handle(_deleteMapMode(Id));
    }

    private async Task _deleteMapMode(int id)
    {
        MapModeGroup? existing = await data.Context.MapModeGroup.SingleOrDefaultAsync(m => m.Id.Equals(id));
        if (existing != null)
        {
            data.Context.MapMode.RemoveRange(existing.MapModes);
            data.Context.MapModeGroup.Remove(existing);
        }
        
        await data.Context.SaveChangesAsync();
    }
    #endregion
}