using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.MenuItems;
using SardCoreAPI.Models.Settings;
using SardCoreAPI.Services.MenuItems;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.MenuItems
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class MenuItemController : GenericController
    {
        private IMenuItemService _menuItemService;
        public MenuItemController(IMenuItemService menuItemService) 
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return new OkObjectResult(await _menuItemService.GetMenuItems());
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [HttpPut]
        [Resource("Library.Setup.Pages")]
        public async Task<IActionResult> Put([FromBody]List<MenuItem> data)
        {
            try
            {
                await _menuItemService.SetMenuItems(data);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
    }
}
