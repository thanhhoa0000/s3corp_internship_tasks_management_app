namespace TaskManagementApp.Frontends.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<AppUserDto>? usersList = new();

            Response? response = await _service.GetUsersAsync();

            if (response!.IsSuccess)
                usersList = JsonSerializer.Deserialize<List<AppUserDto>>(((JsonDocument)response!.Body!).RootElement.GetRawText())!;

            return View(usersList);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid userId)
        {
            AppUserDto model = new();

            Response? response = await _service.GetUserAsync(userId);

            if (response!.IsSuccess)
                model = JsonSerializer.Deserialize<AppUserDto>(((JsonDocument)response.Body!).RootElement.GetRawText())!;
            else
            {
                TempData["error"] = "Error occured when getting user details";

                return RedirectToAction("Index", "User");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid userId)
        {
            Response? response = await _service.DeleteUserAsync(userId);

            if (response!.IsSuccess)
                TempData["success"] = "User deleted successfully!";
            else
                TempData["error"] = "Error occured when deleting User!";

            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateUserRequest model = new();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            if (ModelState.IsValid)
            {
                Response? response = await _service.CreateUserAsync(request);

                if (response!.IsSuccess)
                {
                    TempData["success"] = "Create user successfully!";

                    return RedirectToAction("Index", "User");
                }

                TempData["error"] = "Error occured when creating the user";
            }

            return View(request);
        }

        public async Task<IActionResult> Update(Guid userId)
        {
            Response? response = await _service.GetUserAsync(userId);

            if (!response!.IsSuccess)
            {
                TempData["error"] = "Error occured when getting user!";

                return RedirectToAction("Index", "User");
            }

            var model = JsonSerializer.Deserialize<AppUserDto>(((JsonDocument)response!.Body!).RootElement.GetRawText())!;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AppUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                Response? response = await _service.UpdateUserAsync(userDto);

                if (response!.IsSuccess)
                {
                    TempData["success"] = "Update user successfully!";

                    return RedirectToAction("Details", "User", new { userId = userDto.Id });
                }

                TempData["error"] = "Error occured when creating the user";
            }

            return View(userDto);
        }
    }
}
