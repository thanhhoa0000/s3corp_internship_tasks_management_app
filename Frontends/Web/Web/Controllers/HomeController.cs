using System.Threading.Tasks;

namespace TaskManagementApp.Frontends.Web.Controllers
{
    [Authorize(Roles = "Normal")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaskService _service;

        public HomeController(ILogger<HomeController> logger, ITaskService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<TaskItemDto>? tasksList = new List<TaskItemDto>();

            Response? response = await _service.GetTasksAsync();

            if (response!.IsSuccess)
                tasksList = JsonSerializer.Deserialize<List<TaskItemDto>>(((JsonDocument)response!.Body!).RootElement.GetRawText())!;

            return View(tasksList);
        }

        [HttpGet]
        public async Task<IActionResult> TaskDetail(Guid taskId)
        {
            TaskItemDto model = new();

            Response? response = await _service.GetTaskAsync(taskId);

            if (response!.IsSuccess)
                model = JsonSerializer.Deserialize<TaskItemDto>(((JsonDocument)response.Body!).RootElement.GetRawText())!;
            else 
            {
                TempData["error"] = "Error occured when getting task details";

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
