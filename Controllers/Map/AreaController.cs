using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.Area;
using SardCoreAPI.Utility.Error;
using MySqlConnector;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class AreaController
    {
        private readonly ILogger<AreaController> _logger;

        public AreaController(ILogger<AreaController> logger)
        {
            _logger = logger;
        }

        #region Area
        [HttpGet(Name = "GetAreas")]
        public IActionResult GetAreas([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Area> result = new AreaDataAccess().GetAreas(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetArea")]
        public IActionResult GetArea([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetArea(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostArea")]
        public IActionResult PostArea([FromBody] Area area)
        {
            if (area == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostArea(area))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }

        [HttpPut(Name = "PutArea")]
        public async Task<IActionResult> PutArea([FromBody] Area data)
        {
            int result = await new AreaDataAccess().PutArea(data);
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

        [HttpDelete(Name = "DeleteArea")]
        public async Task<IActionResult> DeleteArea([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            try
            {
                int result = await new AreaDataAccess().DeleteArea((int)Id);
                return result.HandleDelete();
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
        #endregion

        #region Subregion
        [HttpGet(Name = "GetSubregions")]
        public IActionResult GetSubregions([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Subregion> result = new AreaDataAccess().GetSubregions(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetSubregion")]
        public IActionResult GetSubregion([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetSubregion(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostSubregion")]
        public IActionResult PostSubregion([FromBody] Subregion data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostSubregion(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }

        [HttpPut(Name = "PutSubregion")]
        public async Task<IActionResult> PutSubregion([FromBody] Subregion data)
        {
            int result = await new AreaDataAccess().PutSubregion(data);
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

        [HttpDelete(Name = "DeleteSubregion")]
        public async Task<IActionResult> DeleteSubregion([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            try
            {
                int result = await new AreaDataAccess().DeleteSubregion((int)Id);
                return result.HandleDelete();
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
        #endregion

        #region Region
        [HttpGet(Name = "GetRegions")]
        public IActionResult GetRegions([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Region> result = new AreaDataAccess().GetRegions(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetRegion")]
        public IActionResult GetRegion([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetRegion(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostRegion")]
        public IActionResult PostRegion([FromBody] Region data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostRegion(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }

        [HttpPut(Name = "PutRegion")]
        public async Task<IActionResult> PutRegion([FromBody] Region data)
        {
            int result = await new AreaDataAccess().PutRegion(data);
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

        [HttpDelete(Name = "DeleteRegion")]
        public async Task<IActionResult> DeleteRegion([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            try
            {
                int result = await new AreaDataAccess().DeleteRegion((int)Id);
                return result.HandleDelete();
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
        #endregion

        #region Subcontinent
        [HttpGet(Name = "GetSubcontinents")]
        public IActionResult GetSubcontinents([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Subcontinent> result = new AreaDataAccess().GetSubcontinents(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetSubcontinent")]
        public IActionResult GetSubcontinent([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetSubcontinent(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostSubcontinent")]
        public IActionResult PostSubcontinent([FromBody] Subcontinent data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostSubcontinent(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }

        [HttpPut(Name = "PutSubcontinent")]
        public async Task<IActionResult> PutSubcontinent([FromBody] Subcontinent data)
        {
            int result = await new AreaDataAccess().PutSubcontinent(data);
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

        [HttpDelete(Name = "DeleteSubcontinent")]
        public async Task<IActionResult> DeleteSubcontinent([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            try
            {
                int result = await new AreaDataAccess().DeleteSubcontinent((int)Id);
                return result.HandleDelete();
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
        #endregion

        #region Continent
        [HttpGet(Name = "GetContinents")]
        public IActionResult GetContinents([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Continent> result = new AreaDataAccess().GetContinents(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetContinent")]
        public IActionResult GetContinent([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetContinent(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostContinent")]
        public IActionResult PostContinent([FromBody] Continent data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostContinent(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }

        [HttpPut(Name = "PutContinent")]
        public async Task<IActionResult> PutContinent([FromBody] Continent data)
        {
            int result = await new AreaDataAccess().PutContinent(data);
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

        [HttpDelete(Name = "DeleteContinent")]
        public async Task<IActionResult> DeleteContinent([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            try
            {
                int result = await new AreaDataAccess().DeleteContinent((int)Id);
                return result.HandleDelete();
            }
            catch (MySqlException ex) 
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
        #endregion

        #region Celestial Object
        [HttpGet(Name = "GetCelestialObjects")]
        public IActionResult GetCelestialObjects([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<CelestialObject> result = new AreaDataAccess().GetCelestialObjects(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetCelestialObject")]
        public IActionResult GetCelestialObject([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetCelestialObject(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostCelestialObject")]
        public IActionResult PostCelestialObject([FromBody] CelestialObject data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostCelestialObject(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }

        [HttpPut(Name = "PutCelestialObject")]
        public async Task<IActionResult> PutCelestialObject([FromBody] CelestialObject data)
        {
            int result = await new AreaDataAccess().PutCelestialObject(data);
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

        [HttpDelete(Name = "DeleteCelestialObject")]
        public async Task<IActionResult> DeleteCelestialObject([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            try
            {
                int result = await new AreaDataAccess().DeleteCelestialObject((int)Id);
                return result.HandleDelete();
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
        #endregion

        #region Celestial System
        [HttpGet(Name = "GetCelestialSystems")]
        public IActionResult GetCelestialSystems([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<CelestialSystem> result = new AreaDataAccess().GetCelestialSystems(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetCelestialSystem")]
        public IActionResult GetCelestialSystem([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetCelestialSystem(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostCelestialSystem")]
        public IActionResult PostCelestialSystem([FromBody] CelestialSystem data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostCelestialSystem(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }

        [HttpPut(Name = "PutCelestialSystem")]
        public async Task<IActionResult> PutCelestialSystem([FromBody] CelestialSystem data)
        {
            int result = await new AreaDataAccess().PutCelestialSystem(data);
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

        [HttpDelete(Name = "DeleteCelestialSystem")]
        public async Task<IActionResult> DeleteCelestialSystem([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            try
            {
                int result = await new AreaDataAccess().DeleteCelestialSystem((int)Id);
                return result.HandleDelete();
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
        #endregion

        #region Manifold
        [HttpGet(Name = "GetManifolds")]
        public IActionResult GetManifolds([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Manifold> result = new AreaDataAccess().GetManifolds(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetManifold")]
        public IActionResult GetManifold([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetManifold(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostManifold")]
        public IActionResult PostManifold([FromBody] Manifold data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostManifold(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }

        [HttpPut(Name = "PutManifold")]
        public async Task<IActionResult> PutManifold([FromBody] Manifold data)
        {
            int result = await new AreaDataAccess().PutManifold(data);
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

        [HttpDelete(Name = "DeleteManifold")]
        public async Task<IActionResult> DeleteManifold([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            try
            {
                int result = await new AreaDataAccess().DeleteManifold((int)Id);
                return result.HandleDelete();
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
        #endregion
    }
}
