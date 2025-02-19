namespace TaskManagementApp.Frontends.Web.Services
{
    public class AccountService : IAccountService
    {
        private readonly IBaseService _service;

        public AccountService(IBaseService service)
        {
            _service = service;
        }

        public async Task<Response?> LoginAsync(LoginViewModel model)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Post,
                Body = model,
                Url = ApiUrlProperties.AuthUrl + "/api/v1/auth/login"
            }, bearer: false);
        }

        public async Task<Response?> RegisterAsync(RegisterViewModel model)
        {
            return await _service.SendAsync(new Request()
            {
                ApiMethod = ApiMethod.Post,
                Body = model,
                Url = ApiUrlProperties.AuthUrl + "/api/v1/auth/register"
            }, bearer: false);
        }
    }
}
