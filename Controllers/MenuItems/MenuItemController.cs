using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.MenuItems;
using SardCoreAPI.Models.Pages.MenuItems;
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

        [HttpGet]
        [Resource("Library.Setup.Pages")]
        public async Task<IActionResult> GetConfigurableMenuItems()
        {
            return await Handle(_menuItemService.GetConfigurableMenuItems());
        }

        [HttpPut]
        [Resource("Library.Setup.Pages")]
        public async Task<IActionResult> PutConfigurableMenuItems([FromBody] List<ConfigurableMenuItem> data)
        {
            return await Handle(_menuItemService.PutConfigurableMenuItems(data));
        }
    }
}
