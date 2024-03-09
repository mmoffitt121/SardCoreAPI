using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Theming;
using SardCoreAPI.DataAccess.Units;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Theming;
using SardCoreAPI.Models.Units;

namespace SardCoreAPI.Controllers.Units
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class MeasurableController : GenericController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<Measurable> result = await new MeasurableDataAccess().GetMeasurables(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Measurable data)
        {
            if (data == null) { return new BadRequestResult(); }

            bool result = await new MeasurableDataAccess().PostMeasurable(data, WorldInfo);

            if (result)
            {
                return Ok();
            }

            return new BadRequestResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Measurable data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new MeasurableDataAccess().PutMeasurable(data, WorldInfo);

            if (result > 0)
            {
                return Ok();
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

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            int result = await new MeasurableDataAccess().DeleteMeasurable((int)Id, WorldInfo);

            if (result > 0)
            {
                return Ok();
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
