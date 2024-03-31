using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Units;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Units;

namespace SardCoreAPI.Controllers.Units
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class UnitController : GenericController
    {
        [HttpGet]
        [Resource("Library.Setup.Units.Read")]
        public async Task<IActionResult> GetTables([FromQuery] UnitSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<UnitTable> result = await new UnitDataAccess().GetUnitTables(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        [Resource("Library.Setup.Units.Read")]
        public async Task<IActionResult> Get([FromQuery] UnitSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<Unit> result = await new UnitDataAccess().GetUnits(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpPost]
        [Resource("Library.Setup.Units")]
        public async Task<IActionResult> Post([FromBody] Unit data)
        {
            if (data == null) { return new BadRequestResult(); }

            bool result = await new UnitDataAccess().PostUnit(data, WorldInfo);

            if (result)
            {
                return Ok();
            }

            return new BadRequestResult();
        }

        [HttpPut]
        [Resource("Library.Setup.Units")]
        public async Task<IActionResult> Put([FromBody] Unit data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new UnitDataAccess().PutUnit(data, WorldInfo);

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

        [HttpDelete]
        [Resource("Library.Setup.Units")]
        public async Task<IActionResult> Delete([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            int result = await new UnitDataAccess().DeleteUnit((int)Id, WorldInfo);

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
