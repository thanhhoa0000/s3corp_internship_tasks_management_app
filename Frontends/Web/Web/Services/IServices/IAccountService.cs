namespace TaskManagementApp.Frontends.Web.Services.IServices
{
    public interface IAccountService
    {
        Task<Response?> LoginAsync(LoginViewModel model);
        Task<Response?> RegisterAsync(RegisterViewModel model);
    }
}
