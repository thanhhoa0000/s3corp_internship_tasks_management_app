namespace TaskManagementApp.Frontends.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IUserService _service;

        public RoleController(IUserService service, ILogger<RoleController> logger)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<AppRoleDto>? rolesList = new();

            Response? response = await _service.GetRolesAsync();

            if (response!.IsSuccess)
                rolesList = JsonSerializer
                    .Deserialize<List<AppRoleDto>>(
                        ((JsonDocument)response!.Body!).RootElement.GetRawText(), 
                        options: new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;

            return View(rolesList);
        }
    }
}
