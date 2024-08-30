using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Models.Map.Region;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Maps;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class RegionController : GenericController
    {
        private readonly ILogger<RegionController> _logger;
        private readonly IDataService data;
        private readonly ILocationService locationService;

        public RegionController(ILogger<RegionController> logger, IDataService data, ILocationService locationService)
        {
            _logger = logger;
            this.data = data;
            this.locationService = locationService;
        }

        [HttpGet]
        [Resource("Library.Map.Read")]
        public async Task<IActionResult> GetRegions([FromQuery] RegionSearchCriteria criteria)
        {
            return await Handle(data.Context.Region.Where(criteria.GetQuery()).Paginate(criteria).ToListAsync());
        }

        [HttpPost]
        [Resource("Library.Map")]
        public async Task<IActionResult> PostRegion([FromBody] Region region)
        {
            return await Handle(locationService.PutRegion(region));
        }

        [HttpPut]
        [Resource("Library.Map")]
        public async Task<IActionResult> PutRegion([FromBody] Region region)
        {
            return await Handle(locationService.PutRegion(region));
        }

        [HttpDelete]
        [Resource("Library.Map")]
        public async Task<IActionResult> DeleteRegion([FromQuery] int Id)
        {
            return await Handle(locationService.DeleteRegion(Id));
        }
    }
}
