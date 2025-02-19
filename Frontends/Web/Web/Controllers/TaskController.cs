namespace TaskManagementApp.Frontends.Web.Controllers
{
    [Authorize(Roles = "Normal")]
    public class TaskController : Controller
    {
        private readonly ITaskService _service;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService service, ILogger<TaskController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Create()
        {
            TaskItemDto model = new();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItemDto taskDto)
        {
            if (ModelState.IsValid)
            {
                _logger.LogDebug($"User ID when create task: {User.FindFirst(ClaimTypes.NameIdentifier)!.Value}");
                taskDto.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                Response? response = await _service.CreateTaskAsync(taskDto);

                if (response!.IsSuccess)
                {
                    TempData["success"] = "Create task successfully!";

                    return RedirectToAction("Index", "Home");
                }

                TempData["error"] = "Error occured when creating the task";
            }
            
            return View(taskDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid taskId)
        {
            Response? response = await _service.DeleteTaskAsync(taskId);

            if (response!.IsSuccess)
                TempData["success"] = "Task deleted successfully!";
            else
                TempData["error"] = "Error occured when deleting task!";

            return RedirectToAction("Index", "Home");
        }
    }
}
