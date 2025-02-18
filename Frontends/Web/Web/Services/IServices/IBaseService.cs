namespace TaskManagementApp.Frontends.Web.Services.IServices
{
    public interface IBaseService
    {
        Task<Response?> SendAsync<T>(Request request, bool bearer = true) where T : class;
    }
}
