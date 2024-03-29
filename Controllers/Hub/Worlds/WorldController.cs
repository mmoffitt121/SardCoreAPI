﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.DataPoints;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints;
using System.Xml.Linq;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.DataAccess.Hub.Worlds;
using MySqlConnector;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.Hub.Worlds
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class WorldController : GenericController
    {
        private readonly ILogger<DataPointTypeController> _logger;

        public WorldController(ILogger<DataPointTypeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorlds([FromQuery] WorldSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<World> result = await new WorldDataAccess().GetWorlds(criteria);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetWorldCount([FromQuery] WorldSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<World> result = await new WorldDataAccess().GetWorlds(criteria);
            if (result != null)
            {
                return new OkObjectResult(result.Count);
            }
            return new BadRequestResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost]
        public async Task<IActionResult> PostWorld([FromBody] World data)
        {
            if (data == null) { return new BadRequestResult(); }
            data.Normalize();
            if (data.Validate() != null) { return BadRequest(data.Validate()); }

            try
            {
                int result = await new WorldDataAccess().PostWorld(data);

                if (result != 0)
                {
                    World genResult = await new WorldGenerator().GenerateWorld(data);
                    if (genResult != null)
                    {
                        return new OkObjectResult(result);
                    }
                    return new BadRequestResult();
                }

                return new BadRequestResult();
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut]
        public async Task<IActionResult> PutWorld([FromBody] World data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new WorldDataAccess().PutWorld(data);

            if (result != 0)
            {
                return new OkObjectResult(result);
            }

            return new BadRequestResult();
        }
    }
}
