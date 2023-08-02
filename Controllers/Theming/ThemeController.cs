using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Theming;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Theming;
using System.Xml.Linq;

namespace SardCoreAPI.Controllers.Theming
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class ThemeController : GenericController
    {
        [HttpGet(Name = "GetThemes")]
        public async Task<IActionResult> GetThemes([FromQuery] DefaultablePagedSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<Theme> result = await new ThemeDataAccess().GetThemes(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetThemeCount")]
        public async Task<IActionResult> GetThemeCount([FromQuery] DefaultablePagedSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            int result = (await new ThemeDataAccess().GetThemes(criteria, WorldInfo)).Count();
            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost(Name = "PostTheme")]
        public async Task<IActionResult> PostTheme([FromBody] Theme data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new ThemeDataAccess().PostTheme(data, WorldInfo);

            if (result != 0)
            {
                return new OkObjectResult(result);
            }

            return new BadRequestResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut(Name = "PutTheme")]
        public async Task<IActionResult> PutTheme([FromBody] Theme data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new ThemeDataAccess().PutTheme(data, WorldInfo);

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

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete(Name = "DeleteTheme")]
        public async Task<IActionResult> DeleteTheme([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            int result = await new ThemeDataAccess().DeleteTheme((int)Id, WorldInfo);

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
