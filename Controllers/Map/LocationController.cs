﻿using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.Location;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class LocationController
    {
        private readonly ILogger<MapController> _logger;

        public LocationController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetLocations")]
        public IActionResult GetLocations([FromQuery] LocationSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<Location> result = new LocationDataAccess().GetLocations(criteria);
            if (result != null) 
            { 
                return new OkObjectResult(result); 
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetLocation")]
        public IActionResult GetLocation([FromQuery] int? Id)
        {
            Location result = new LocationDataAccess().GetLocation(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostLocation")]
        public IActionResult PostLocation([FromBody] Location location)
        {
            if (location == null) { return new BadRequestResult(); }

            if (new LocationDataAccess().PostLocation(location))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
    }
}
