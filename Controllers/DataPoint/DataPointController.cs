using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.Map;

namespace SardCoreAPI.Controllers.DataPoint
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointController : Controller
    {
        private readonly ILogger<MapController> _logger;

        public DataPointController(ILogger<MapController> logger)
        {
            _logger = logger;
        }
    }
}
