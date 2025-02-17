namespace TaskManagementApp.Frontends.Web.Services.IServices
{
    public interface IBaseService
    {
        Task<Response?> SendAsync(Request request, bool bearer = true);
    }
}
