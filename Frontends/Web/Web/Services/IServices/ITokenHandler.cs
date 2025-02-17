namespace TaskManagementApp.Frontends.Web.Services.IServices
{
    public interface ITokenHandler
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
    }
}
