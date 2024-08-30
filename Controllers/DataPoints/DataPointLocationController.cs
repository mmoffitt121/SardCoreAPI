using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.DataPoints;
using SardCoreAPI.Services.Maps;
using SardCoreAPI.Utility.DataAccess;
using SardCoreAPI.Utility.Error;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;

namespace SardCoreAPI.Controllers.DataPoints
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointLocationController : GenericController
    {
        private readonly ILogger<MapController> _logger;
        private readonly IDataService data;
        private readonly ILocationService locationService;
        private readonly IDataPointService dataPointService;

        public DataPointLocationController(ILogger<MapController> logger, IDataService data, ILocationService locationService, IDataPointService dataPointService)
        {
            _logger = logger;
            this.data = data;
            this.locationService = locationService;
            this.dataPointService = dataPointService;
        }

        [HttpGet]
        [Resource("Library.Document.Read")]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetDataPointsFromLocationId([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(dataPointService.GetDataPoints(new DataPointSearchCriteria()
            {
                LocationIds = new List<int>() { criteria.Id ?? -1 }
            }));
        }

        [HttpGet]
        [Resource("Library.Document.Read")]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocationsFromDataPointId([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(data.Context.DataPointLocation
                .Where(x => x.DataPointId.Equals(criteria.Id))
                .Paginate(criteria)
                .Include(dpl => dpl.Location)
                .Select(dpl => dpl.Location)
                .ToListAsync());
        }

        [HttpPost]
        [Resource("Library.Document")]
        [Resource("Library.Location")]
        public async Task<IActionResult> PutDataPointLocation([FromBody] DataPointLocation dpl)
        {
            return await Handle(locationService.PutDataPointLocation(dpl));
        }

        [HttpPost]
        [Resource("Library.Document")]
        [Resource("Library.Location")]
        public async Task<IActionResult> DeleteDataPointLocation([FromBody] DataPointLocation dpl)
        {
            return await Handle(data.Context.DataPointLocation.Where(x => x.LocationId.Equals(dpl.LocationId) && x.DataPointId.Equals(dpl.DataPointId)).ExecuteDeleteAsync());
        }
    }
}
