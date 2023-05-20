using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.Map;

namespace SardCoreAPI.Controllers.DataPoint
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointTypeController
    {
        private readonly ILogger<MapController> _logger;

        public DataPointTypeController(ILogger<MapController> logger)
        {
            _logger = logger;
        }
    }
}
