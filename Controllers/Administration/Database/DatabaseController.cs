using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Administration.Database;
using SardCoreAPI.DataAccess.Content;
using SardCoreAPI.Models.Content;

namespace SardCoreAPI.Controllers.Administration.Database
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DatabaseController : Controller
    { 
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetServerVersion()
        {
            return Ok(new { Version = await new DatabaseDataAccess().GetServerVersion() });
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetDatabases()
        {
            return Ok(await new DatabaseDataAccess().GetDatabases());
        }
    }
}
