using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Units;
using SardCoreAPI.Services.Tasks;
using SardCoreAPI.Services.WorldContext;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SardCoreAPI.Controllers.Tasks
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class TaskController : GenericController
    {
        private TaskService _taskService;
        private IWorldInfoService _worldInfoService;

        public TaskController(TaskService taskService, IWorldInfoService worldInfoService)
        {
            _taskService = taskService;
            _worldInfoService = worldInfoService;
        }

        [HttpGet]
        [Resource("Library.Setup.Read")]
        public async Task<IActionResult> GetTasks()
        {
            return await Handle(_taskService.GetTasks(_worldInfoService.WorldLocation));
        }

        [HttpDelete]
        [Resource("Library.Setup")]
        public async Task<IActionResult> CancelTask(string taskId)
        {
            SardTask? task = (await _taskService.GetTasks(_worldInfoService.WorldLocation)).Where(t => t.Id.Equals(taskId)).SingleOrDefault();

            if (task == null)
            {
                return Ok();
            }
            else if (task.WorldLocation.Equals(_worldInfoService.WorldLocation))
            {
                _taskService.Cancel(taskId);
                return Ok();
            }
            else
            {
                return new BadRequestObjectResult("Task not found.");
            }
        }
    }
}
