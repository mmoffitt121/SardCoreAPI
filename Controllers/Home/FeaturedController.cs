using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Home;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Home;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.Home
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class FeaturedController : GenericController
    {
        [HttpGet]
        [Resource("Library.General.Read")]
        public async Task<IActionResult> GetFeatured()
        {
            return Ok(await new FeaturedDataAccess().GetFeatured(WorldInfo));
        }

        [HttpGet]
        [Resource("Library.General.Read")]
        public async Task<IActionResult> GetFeaturedFromWorld(string worldLocation)
        {
            try
            {
                WorldInfo wInfo = new WorldInfo(worldLocation);
                return Ok(await new FeaturedDataAccess().GetFeatured(wInfo));
            }
            catch (MySqlException e)
            {
                return e.Handle();
            }
            catch (Exception e)
            {
                return e.Handle();
            }
            
        }

        
        [HttpPost]
        [Resource("Library.General")]
        public async Task<IActionResult> UpdateFeatured(FeaturedEditRequest request)
        {
            if (request == null) { return new BadRequestResult(); }

            WorldInfo wInfo = new WorldInfo(request.WorldLocation);

            await new FeaturedDataAccess().PurgeFeatured(wInfo);

            int result = await new FeaturedDataAccess().UpdateFeatured(request.Featured, wInfo);

            return Ok();
        }
    }
}
