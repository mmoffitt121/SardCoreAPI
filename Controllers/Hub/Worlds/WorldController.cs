using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Services.Context;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Services.Hub;
using SardCoreAPI.Utility.Validation;

namespace SardCoreAPI.Controllers.Hub.Worlds
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class WorldController : GenericController
    {
        private readonly ILogger<DataPointTypeController> _logger;
        private readonly IDataService _data;
        private readonly IWorldService _worldService;

        public WorldController(ILogger<DataPointTypeController> logger, IDataService data, IWorldService worldService)
        {
            _logger = logger;
            _data = data;
            _worldService = worldService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorlds([FromQuery] WorldSearchCriteria criteria)
        {
             if (criteria == null) { return new BadRequestResult(); }

             return await Handle(_data.CoreContext.World.Where(criteria.GetQuery()).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetWorldCount([FromQuery] WorldSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            return await Handle(_data.CoreContext.World.Where(criteria.GetQuery()).CountAsync());
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost]
        [Validate]
        public async Task<IActionResult> PostWorld([FromBody] World data)
        {
            return await Handle(_worldService.CreateWorld(data));
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut]
        [Validate]
        public async Task<IActionResult> PutWorld([FromBody] World data)
        {
            _data.CoreContext.World.Update(data);
            return await Handle(_data.CoreContext.SaveChangesAsync());
        }
    }
}
