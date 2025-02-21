namespace TaskManagementApp.Frontends.Web.Controllers
{
    [Authorize(Roles = "Normal")]
    public class TaskController : Controller
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
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

        public async Task<IActionResult> Update(Guid taskId)
        {
            Response? response = await _service.GetTaskAsync(taskId);

            if (!response!.IsSuccess)
            {
                TempData["error"] = "Error occured when getting task!";

                return RedirectToAction("Index", "Home");
            }

            var model = JsonSerializer.Deserialize<TaskItemDto>(((JsonDocument)response!.Body!).RootElement.GetRawText())!;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TaskItemDto taskDto)
        {
            if (ModelState.IsValid)
            {
                Response? response = await _service.UpdateTaskAsync(taskDto);

                if (response!.IsSuccess)
                {
                    TempData["success"] = "Update task successfully!";

                    return RedirectToAction("TaskDetail", "Home", new { taskId = taskDto.Id });
                }

                TempData["error"] = "Error occured when creating the task";
            }

            return View(taskDto);
        }
    }
}
