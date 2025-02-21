namespace TaskManagementApp.Frontends.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _service;
        private readonly ITokenProcessor _tokenHandler;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService service, ITokenProcessor tokenHandler, ILogger<AccountController> logger)
        {
            _service = service;
            _tokenHandler = tokenHandler;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            LoginViewModel model = new();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Response? response = await _service.LoginAsync(model);

            _logger.LogDebug($"Account controller response: {response!.Body ?? response!.Message}");

            if (response is null)
            {
                TempData["error"] = "Invalid login response from server.";
                return View(model);
            }

            if (response.Body is not null && response.IsSuccess)
            {
                LoginResponse loginResponse 
                    = JsonSerializer.Deserialize<LoginResponse>(((JsonDocument)response.Body!).RootElement.GetRawText())!;


                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(loginResponse.Token);

                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value ?? "";
                var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "name")?.Value ?? "";

                if (roleClaim.ToString().IsNullOrEmpty())
                {
                    TempData["error"] = "Error(s) occured!";

                    return View(model);
                }

                await SignUserIn(loginResponse);

                _tokenHandler.SetToken(loginResponse.Token);

                if (roleClaim == "Admin")
                {
                    TempData["success"] = $"Hello admin {nameClaim}";

                    return RedirectToAction("Index", "User");
                }
                else if (roleClaim == "Normal")
                {
                    TempData["success"] = $"Hello {nameClaim}";

                    return RedirectToAction("Index", "Home");
                }

                TempData["error"] = $"Invalid login response from server: {response.Message}";

                return View(model);
            }

            if (response.Message.Contains("Username or password is wrong!"))
            {
                TempData["error"] = response.Message;
            }
            else
            {
                TempData["error"] = $"Invalid login response from server: {response.Message}";
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
            => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Response? response = await _service.RegisterAsync(model);

            if (response is not null && response.IsSuccess)
            {
                if (model.Role.ToString().IsNullOrEmpty())
                    model.Role = Role.Normal;

                TempData["success"] = "Registering successfully!";

                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = response!.Message;

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenHandler.ClearToken();

            return RedirectToAction("Login", "Account");
        }

        private async Task SignUserIn(LoginResponse response)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(response.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)!.Value));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)!.Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)!.Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)!.Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name)!.Value));
            identity.AddClaim(new Claim(ClaimTypes.Name,
                $"{response.User.FirstName} {response.User.LastName}"));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwtToken.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role)!.Value));

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
