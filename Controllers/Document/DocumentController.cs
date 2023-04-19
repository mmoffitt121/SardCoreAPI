using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Models.Document.SearchCriteria;
using SardCoreAPI.Models;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.Models.Document;

namespace SardCoreAPI.Controllers.Document
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DocumentController
    {
        private readonly ILogger<MapController> _logger;

        public DocumentController(ILogger<MapController> logger)
        {
            _logger = logger;
        }
    }
}
