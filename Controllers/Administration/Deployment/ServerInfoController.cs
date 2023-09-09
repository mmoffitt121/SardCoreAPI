using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Administration.Database;

namespace SardCoreAPI.Controllers.Administration.Deployment
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class ServerInfoController : GenericController
    {
        [HttpGet]
        public async Task<IActionResult> GetServerVersion()
        {
            return Ok(new { Version = "0.0.0" });
        }
    }
}
