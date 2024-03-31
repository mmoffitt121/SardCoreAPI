using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.Region;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class RegionController : GenericController
    {
        private readonly ILogger<RegionController> _logger;

        public RegionController(ILogger<RegionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Resource("Library.Map.Read")]
        public async Task<IActionResult> GetRegions([FromQuery] RegionSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<Region> result = await new RegionDataAccess().GetRegions(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpPost]
        [Resource("Library.Map")]
        public async Task<IActionResult> PostRegion([FromBody] Region data)
        {
            if (data == null) { return new BadRequestResult(); }

            return Ok(await new RegionDataAccess().PostRegion(data, WorldInfo));
        }

        [HttpPut]
        [Resource("Library.Map")]
        public async Task<IActionResult> PutRegion([FromBody] Region data)
        {
            int result = await new RegionDataAccess().PutRegion(data, WorldInfo);
            if (result > 0)
            {
                return new OkResult();
            }
            else if (result == 0)
            {
                return new NotFoundResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpDelete]
        [Resource("Library.Map")]
        public async Task<IActionResult> DeleteRegion([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            int result = await new RegionDataAccess().DeleteRegion((int)Id, WorldInfo);

            if (result > 0)
            {
                return new OkResult();
            }
            else if (result == 0)
            {
                return new NotFoundResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
