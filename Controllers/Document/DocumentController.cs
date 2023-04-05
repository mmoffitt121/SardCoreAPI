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
        /*
        [HttpGet(Name = "GetDocuments")]
        public IEnumerable<Document> GetDocuments([FromQuery] MapSearchCriteria criteria)
        {
            return MapCode.GetMaps(criteria);
        }

        [HttpGet(Name = "GetMap")]
        public Map? GetMap(int mapid)
        {
            return MapCode.GetMap(mapid);
        }

        [HttpPost(Name = "PostMap")]
        public string PostMap([FromBody] Map map)
        {
            if (MapCode.PostMap(map))
            {
                return "Operation was a success";
            }

            return "Operation failed";
        }

        [HttpPut(Name = "PutMap")]
        public string PutMaps(Map map)
        {
            if (MapCode.PutMap(map))
            {
                return "Operation was a success";
            }

            return "Operation failed";
        }

        [HttpDelete(Name = "DeleteMap")]
        public string DeleteMap(int id)
        {
            if (MapCode.DeleteMap(id))
            {
                return "Operation was a success";
            }

            return "Operation failed";
        }*/
    }
}
