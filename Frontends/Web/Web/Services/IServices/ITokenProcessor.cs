namespace TaskManagementApp.Frontends.Web.Services.IServices
{
    public interface ITokenProcessor
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
    }
}
