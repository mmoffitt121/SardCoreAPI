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

        public DataPointLocationController(ILogger<MapController> logger, IDataService data)
        {
            _logger = logger;
            this.data = data;
        }

        [HttpGet]
        [Resource("Library.Document.Read")]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetDataPointsFromLocationId([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(data.Context.DataPointLocation.Where(x => x.LocationId.Equals(criteria.Id)).Paginate(criteria).ToListAsync());
        }

        [HttpGet]
        [Resource("Library.Document.Read")]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetDataPointsFromLocationIdCount([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(data.Context.DataPointLocation.Where(x => x.LocationId.Equals(criteria.Id)).CountAsync());
        }

        [HttpGet]
        [Resource("Library.Document.Read")]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocationsFromDataPointId([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(data.Context.DataPointLocation.Where(x => x.DataPointId.Equals(criteria.Id)).Paginate(criteria).ToListAsync());
        }

        [HttpGet]
        [Resource("Library.Document.Read")]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocationsFromDataPointIdCount([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(data.Context.DataPointLocation.Where(x => x.DataPointId.Equals(criteria.Id)).CountAsync());
        }

        [HttpPost]
        [Resource("Library.Document")]
        [Resource("Library.Location")]
        public async Task<IActionResult> PostDataPointLocation([FromBody] DataPointLocation dpl)
        {
            return await Handle(locationService.PostDataPointLocation(dpl));
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
