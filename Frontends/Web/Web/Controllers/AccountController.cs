using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace TaskManagementApp.Frontends.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _service;
        private readonly ITokenHandler _tokenHandler;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService service, ITokenHandler tokenHandler, ILogger<AccountController> logger)
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

            if (response is null)
            {
                TempData["error"] = "Invalid login response from server.";
                return View(model);
            }

            if (response is not null && response.IsSuccess)
            {
                LoginResponse loginResponse 
                    = JsonSerializer.Deserialize<LoginResponse>(Convert.ToString(response.Body)!)!;

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(loginResponse.Token);

                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value ?? "";

                if (roleClaim.ToString().IsNullOrEmpty())
                {
                    TempData["error"] = "Error(s) occured!";

                    return View(model);
                }

                await SignUserIn(loginResponse);

                _tokenHandler.SetToken(loginResponse.Token);

                if (roleClaim == "Admin")
                    return RedirectToAction("Index", "Users");
                else if (roleClaim == "Normal")
                    return RedirectToAction("Index", "Home");

                TempData["error"] = response?.Message ?? "Login failed. Please try again.";

                return View(model);
            }

            TempData["error"] = "Invalid login response from server.";

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
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)!.Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name)!.Value));
            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)!.Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwtToken.Claims.FirstOrDefault(u => u.Type == "role")!.Value));

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
