using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Maps;
using SardCoreAPI.Utility.DataAccess;
using System.Runtime.CompilerServices;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class LocationController : GenericController
    {
        private readonly ILogger<MapController> _logger;
        private readonly IDataService data;
        private readonly ILocationService locationService;

        public LocationController(ILogger<MapController> logger, IDataService data, ILocationService locationService)
        {
            _logger = logger;
            this.data = data;
            this.locationService = locationService;
        }

        [HttpGet(Name = "GetLocations")]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocations([FromQuery] LocationSearchCriteria criteria)
        {
            return await Handle(locationService.GetLocations(criteria));
        }

        [HttpGet]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocationsCount([FromQuery] LocationSearchCriteria criteria)
        {
            return await Handle(locationService.CountLocations(criteria));
        }

        [HttpGet(Name = "GetLocation")]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocation([FromQuery] int Id)
        {
            return await Handle(locationService.GetLocation(Id));
        }

        [HttpGet]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocationHeiarchy(int id, int depth)
        {
            return await Handle(locationService.GetLocationHeiarchy(id, depth));
        }

        [HttpPost(Name = "PostLocation")]
        [Resource("Library.Location")]
        public async Task<IActionResult> PostLocation([FromBody] Location location)
        {
            return await Handle(locationService.PutLocation(location));
        }

        [HttpPut(Name = "PutLocation")]
        [Resource("Library.Location")]
        public async Task<IActionResult> PutLocation([FromBody] Location location)
        {
            return await Handle(locationService.PutLocation(location));
        }

        [HttpDelete(Name = "DeleteLocation")]
        [Resource("Library.Location")]
        public async Task<IActionResult> DeleteLocation([FromQuery] int Id)
        {
            return await Handle(locationService.DeleteLocation(Id));
        }
    }
}
