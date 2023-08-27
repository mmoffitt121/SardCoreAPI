using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Utility.Error;
using System.Xml.Linq;

namespace SardCoreAPI.Controllers.DataPoints
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointLocationController : GenericController
    {
        private readonly ILogger<MapController> _logger;

        public DataPointLocationController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDataPointsFromLocationId([FromQuery] PagedSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<DataPoint> result = await new DataPointLocationDataAccess().GetDataPointsFromLocationId(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetDataPointsFromLocationIdCount([FromQuery] PagedSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            int result = await new DataPointLocationDataAccess().GetDataPointsFromLocationIdCount(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetLocationsFromDataPointId([FromQuery] PagedSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<Location> result = await new DataPointLocationDataAccess().GetLocationsFromDataPointId(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetLocationsFromDataPointIdCount([FromQuery] PagedSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            int result = await new DataPointLocationDataAccess().GetLocationsFromDataPointIdCount(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost]
        public async Task<IActionResult> PostDataPointLocation([FromBody] DataPointLocation location)
        {
            if (location == null) { return new BadRequestResult(); }

            try
            {
                await new DataPointLocationDataAccess().PostDataPointLocation(location, WorldInfo);
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }

            return Ok();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost]
        public async Task<IActionResult> DeleteDataPointLocation([FromBody] DataPointLocation location)
        {
            if (location == null) { return new BadRequestResult(); }

            try
            {
                await new DataPointLocationDataAccess().DeleteDataPointLocation(location, WorldInfo);
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }

            return Ok();
        }
    }
}
