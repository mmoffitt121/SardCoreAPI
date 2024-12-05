using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Maps;
using SardCoreAPI.Utility.DataAccess;
using SardCoreAPI.Utility.Map;
using System.Reflection.Emit;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class LocationTypeController : GenericController
    {
        private readonly ILogger<MapController> _logger;
        private readonly IDataService data;
        private readonly ILocationService locationService;

        public LocationTypeController(ILogger<MapController> logger, IDataService dataService, ILocationService locationService)
        {
            _logger = logger;
            data = dataService;
            this.locationService = locationService;
        }

        [HttpGet(Name = "GetLocationTypes")]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocationTypes([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(data.Context.LocationType
                .Where(lt => lt.Name.Contains(criteria.Query ?? ""))
                .Where(lt => criteria.Id != null ? lt.Id == criteria.Id : true)
                .Paginate(criteria)
                .OrderBy(lt => lt.Name)
                .ToListAsync());
        }


        [HttpGet]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocationTypeCount([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(data.Context.LocationType
                .Where(lt => lt.Name.Contains(criteria.Query ?? ""))
                .CountAsync());
        }


        [HttpGet(Name = "GetLocationType")]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocationType(int id)
        {
            return await Handle(data.Context.LocationType.SingleAsync(lt => lt.Id.Equals(id)));
        }

        [HttpPost(Name = "PostLocationType")]
        [Resource("Library.Location")]
        public async Task<IActionResult> PostLocationType([FromBody] LocationType type)
        {
            return await Handle(locationService.PutLocationType(type));
        }

        [HttpPut(Name = "PutLocationType")]
        [Resource("Library.Location")]
        public async Task<IActionResult> PutLocationType([FromBody] LocationType type)
        {
            return await Handle(locationService.PutLocationType(type));
        }

        [HttpDelete]
        [Resource("Library.Location")]
        public async Task<IActionResult> DeleteLocationType(int Id)
        {
            return await Handle(locationService.DeleteLocationType(Id));
        }
    }
}
